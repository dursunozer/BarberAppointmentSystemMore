using BarberAppointmentSystem.Data;
using BarberAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberAppointmentSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AppointmentController : Controller
    {
        private readonly BarberContext _context;

        public AppointmentController(BarberContext context)
        {
            _context = context;
        }

        // Randevu alma ekranı
        public async Task<IActionResult> BookAppointment()
        {
            var employees = await _context.Employees
                .Where(e => e.IsAvailable)
                .ToListAsync();

            ViewBag.Employees = employees;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(int employeeId, DateTime appointmentDate, int serviceId, int customerId)
        {
            var employee = await _context.Employees
                .Include(e => e.Appointments)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null || !employee.IsAvailable)
            {
                ModelState.AddModelError("", "Çalışan uygun değil.");
                return RedirectToAction(nameof(BookAppointment));
            }

            // Yeni randevu oluştur
            var appointment = new Appointment
            {
                EmployeeId = employeeId,
                AppointmentDate = appointmentDate,
                ServiceId = serviceId,
                CustomerId = customerId,
                Status = "Pending",
            };

            employee.IsAvailable = false;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Customer");
        }
    }
}
