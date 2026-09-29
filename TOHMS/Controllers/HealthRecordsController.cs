using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace TOHMS.Controllers
{
    [Authorize(Roles = "Administrator,Occupational Health Practitioner")]
    public class HealthRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _context.HealthRecords
                .Include(h => h.Employee)
                .ThenInclude(e => e.Client)
                .OrderByDescending(h => h.RecordDate)
                .ToListAsync();

            return View(records);
        }

        public async Task<IActionResult> Details(int id)
        {
            var record = await _context.HealthRecords
                .Include(h => h.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(
                    h => h.HealthRecordId == id);

            if (record == null)
                return NotFound();

            return View(record);
        }

        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            HealthRecord healthRecord)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(
                    healthRecord.EmployeeId);

                return View(healthRecord);
            }

            _context.HealthRecords.Add(healthRecord);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Health record successfully created.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record =
                await _context.HealthRecords.FindAsync(id);

            if (record == null)
                return NotFound();

            await LoadEmployeesAsync(record.EmployeeId);

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            HealthRecord healthRecord)
        {
            if (id != healthRecord.HealthRecordId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(
                    healthRecord.EmployeeId);

                return View(healthRecord);
            }

            _context.Update(healthRecord);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Health record successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.HealthRecords
                .Include(h => h.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(
                    h => h.HealthRecordId == id);

            if (record == null)
                return NotFound();

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record =
                await _context.HealthRecords.FindAsync(id);

            if (record != null)
            {
                _context.HealthRecords.Remove(record);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Health record successfully deleted.";

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