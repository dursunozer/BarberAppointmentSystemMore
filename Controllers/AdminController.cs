using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberAppointmentSystem.Data;
using BarberAppointmentSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace BarberAppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BarberContext _context;

        public AdminController(BarberContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.DailyEarnings = _context.Appointments
                .Where(a => a.AppointmentDate == DateTime.Today)
                .Sum(a => a.Service.Price);

            ViewBag.Employees = _context.Employees.Select(e => new
            {
                e.EmployeeId,
                e.FirstName,
                e.LastName,
                e.Role,
                Status = e.IsAvailable ? "Uygun" : "Meşgul"
            }).ToList();

            return View();
        }

        // GET: Admin/CustomerList
        public async Task<IActionResult> CustomerList()
        {
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        // GET: Admin/AddCustomer
        public IActionResult AddCustomer()
        {
            return View();
        }

        // POST: Admin/AddCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerList));
            }
            return View(customer);
        }

        // GET: Admin/EditCustomer/Id
        public async Task<IActionResult> EditCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int CustomerId, [Bind("CustomerId,FirstName,LastName,Email,Phone")] Customer customer)
        {
            if (CustomerId != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers.FindAsync(CustomerId);
                    if (existingCustomer == null)
                    {
                        return NotFound();
                    }

                    // Mevcut müşteri bilgilerini güncelle
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Phone = customer.Phone;

                    // Değişiklikleri kaydet
                    _context.Customers.Update(existingCustomer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(CustomerList));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        // GET: Admin/EmployeeList
        public async Task<IActionResult> EmployeeList()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        // GET: Admin/AddEmployee
        public IActionResult AddEmployee()
        {
            return View();
        }

        // POST: Admin/AddEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.IsAvailable = true; // Default availability
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EmployeeList));
            }
            return View(employee);
        }

        // GET: Admin/EditEmployee/Id
        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Admin/EditEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int id, [Bind("EmployeeId,FirstName,LastName,Role,IsAvailable")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(EmployeeList));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
