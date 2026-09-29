using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;


namespace TOHMS.Controllers
{
    [Authorize(Roles = "Administrator,Occupational Health Practitioner")]
    public class CertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var certificates = await _context.Certificates
                .Include(c => c.Employee)
                .ThenInclude(e => e.Client)
                .OrderByDescending(c => c.IssueDate)
                .ToListAsync();

            return View(certificates);
        }

        public async Task<IActionResult> Details(int id)
        {
            var certificate = await _context.Certificates
                .Include(c => c.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(c => c.CertificateId == id);

            if (certificate == null)
                return NotFound();

            return View(certificate);
        }

        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate certificate)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(certificate.EmployeeId);
                return View(certificate);
            }

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Certificate successfully created.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var certificate =
                await _context.Certificates.FindAsync(id);

            if (certificate == null)
                return NotFound();

            await LoadEmployeesAsync(certificate.EmployeeId);

            return View(certificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Certificate certificate)
        {
            if (id != certificate.CertificateId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(certificate.EmployeeId);
                return View(certificate);
            }

            _context.Update(certificate);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Certificate successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var certificate = await _context.Certificates
                .Include(c => c.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(c => c.CertificateId == id);

            if (certificate == null)
                return NotFound();

            return View(certificate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificate =
                await _context.Certificates.FindAsync(id);

            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Certificate successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadEmployeesAsync(
            int? selectedEmployeeId = null)
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .Select(e => new
                {
                    e.EmployeeId,
                    DisplayName =
                        e.FirstName + " " +
                        e.LastName + " - " +
                        e.EmployeeNumber
                })
                .ToListAsync();

            ViewBag.Employees = new SelectList(
                employees,
                "EmployeeId",
                "DisplayName",
                selectedEmployeeId);
        }
    }
}