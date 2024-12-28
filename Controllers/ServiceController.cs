using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberAppointmentSystem.Data;
using BarberAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace BarberAppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly BarberContext _context;

        public ServiceController(BarberContext context)
        {
            _context = context;
        }

        // GET: Service/Index
        public async Task<IActionResult> Index()
        {
            // Servisleri ve atanmış çalışanları dahil ederek listele
            var services = await _context.Services
                .Include(s => s.Employees) // Çalışanları da dahil et
                .ToListAsync();
            return View(services);
        }

        // GET: Service/Create
        public IActionResult Create()
        {
            // Çalışanları dropdown veya checkbox için gönder
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Services service, int[] employeeIds)
        {
            if (ModelState.IsValid)
            {
                // Servisi ekle
                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                // Çalışanları servise ata
                var employees = _context.Employees
                    .Where(e => employeeIds.Contains(e.EmployeeId))
                    .ToList();
                service.Employees = employees;

                _context.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(service);
        }

        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Servis ve ilişkili çalışanları getir
            var service = await _context.Services
                .Include(s => s.Employees)
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound();

            // Tüm çalışanları ViewBag'e ata
            ViewBag.Employees = await _context.Employees.ToListAsync() ?? new List<Employee>(); // Boş bir liste döndür

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Services service, int[] employeeIds)
        {
            if (id != service.ServiceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut servisi getir
                    var existingService = await _context.Services
                        .Include(s => s.Employees)
                        .FirstOrDefaultAsync(s => s.ServiceId == id);

                    if (existingService == null)
                        return NotFound();

                    // Servis bilgilerini güncelle
                    existingService.Name = service.Name;
                    existingService.Description = service.Description;
                    existingService.Price = service.Price;

                    // Çalışan ilişkilerini güncelle
                    existingService.Employees.Clear(); // Eski çalışanları kaldır
                    if (employeeIds != null && employeeIds.Any())
                    {
                        var employees = await _context.Employees
                            .Where(e => employeeIds.Contains(e.EmployeeId))
                            .ToListAsync();
                        existingService.Employees = employees;
                    }

                    // Değişiklikleri kaydet
                    _context.Update(existingService);
                    await _context.SaveChangesAsync();

                    // Başarılı işlem sonrası Index'e yönlendir
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata durumunda log kaydı yap ve sayfaya geri dön
                    Console.WriteLine($"Hata: {ex.Message}");
                    ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                }
            }

            // Hatalı durumda çalışan listesiyle aynı sayfaya geri dön
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
