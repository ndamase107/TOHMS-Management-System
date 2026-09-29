using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace TOHMS.Controllers
{
    [Authorize(Roles = "Administrator,Occupational Health Practitioner")]
    public class ExaminationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExaminationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var examinations = await _context.Examinations
                .Include(e => e.Employee)
                .ThenInclude(e => e.Client)
                .OrderByDescending(e => e.ExaminationDate)
                .ToListAsync();

            return View(examinations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var examination = await _context.Examinations
                .Include(e => e.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(e => e.ExaminationId == id);

            if (examination == null)
                return NotFound();

            return View(examination);
        }

        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Examination examination)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(examination.EmployeeId);
                return View(examination);
            }

            _context.Examinations.Add(examination);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Examination successfully recorded.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var examination =
                await _context.Examinations.FindAsync(id);

            if (examination == null)
                return NotFound();

            await LoadEmployeesAsync(examination.EmployeeId);

            return View(examination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Examination examination)
        {
            if (id != examination.ExaminationId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(examination.EmployeeId);
                return View(examination);
            }

            _context.Update(examination);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Examination successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var examination = await _context.Examinations
                .Include(e => e.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(e => e.ExaminationId == id);

            if (examination == null)
                return NotFound();

            return View(examination);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examination =
                await _context.Examinations.FindAsync(id);

            if (examination != null)
            {
                _context.Examinations.Remove(examination);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Examination successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadEmployeesAsync(
            int? selectedEmployeeId = null)
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Select(e => new
                {
                    e.EmployeeId,
                    DisplayName = e.FirstName + " " +
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