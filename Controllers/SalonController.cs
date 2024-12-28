using BarberAppointmentSystem.Data;
using BarberAppointmentSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BarberAppointmentSystem.Controllers
{
    public class SalonController : Controller
    {
        private readonly BarberContext _context;

        public SalonController(BarberContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Email ve şifre boşluk kontrolü
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Hata"] = "Email ve şifre boş bırakılamaz!";
                return RedirectToAction("Login");
            }

            // Admin Kontrolü
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (admin != null)
            {
                await SignInUser(admin.Email, "Admin");
                return RedirectToAction("Dashboard", "Admin");
            }

            // Customer Kontrolü
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);
            if (customer != null)
            {
                await SignInUser(customer.Email, "Customer");
                return RedirectToAction("CustomerSession");
            }

            // Employee Kontrolü
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email && e.Password == password);
            if (employee != null)
            {
                await SignInUser(employee.Email, "Employee");
                return RedirectToAction("EmployeeSession");
            }

            // Kullanıcı bulunamadıysa hata mesajı
            TempData["Hata"] = "Kullanıcı bilgileri hatalı. Lütfen kontrol ediniz.";
            return RedirectToAction("Login");
        }

        // Kullanıcı Oturum Açma İşlemi
        private async Task SignInUser(string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        // Admin Session
        [HttpGet]
        public IActionResult AdminSession()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }

            TempData["Hata"] = "Yalnızca admin kullanıcıları bu sayfaya erişebilir!";
            return RedirectToAction("Login");
        }

        // Customer Session
        [HttpGet]
        public IActionResult CustomerSession()
        {
            if (User.IsInRole("Customer"))
            {
                return View();
            }

            TempData["Hata"] = "Yalnızca müşteri kullanıcıları bu sayfaya erişebilir!";
            return RedirectToAction("Login");
        }

        // Employee Session
        [HttpGet]
        public IActionResult EmployeeSession()
        {
            if (User.IsInRole("Employee"))
            {
                return View();
            }

            TempData["Hata"] = "Yalnızca çalışan kullanıcıları bu sayfaya erişebilir!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Mesaj"] = "Oturum başarıyla sonlandırıldı!";
            return RedirectToAction("Login");
        }


        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Müşteri başarıyla eklendi.";
                return RedirectToAction(nameof(CustomerSession));
            }

            TempData["Error"] = "Müşteri eklenirken bir hata oluştu.";
            return View(customer);
        }
    }
}
