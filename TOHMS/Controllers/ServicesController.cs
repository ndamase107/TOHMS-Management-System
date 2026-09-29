using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace TOHMS.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Services
                .OrderBy(s => s.ServiceName)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var service =
                await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
                return View(service);

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Service successfully created.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service =
                await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Service service)
        {
            if (id != service.ServiceId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(service);

            _context.Update(service);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Service successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var service =
                await _context.Services.FindAsync(id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service =
                await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Service successfully deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}