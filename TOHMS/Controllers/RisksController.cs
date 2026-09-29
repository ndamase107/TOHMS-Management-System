using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace TOHMS.Controllers
{
    [Authorize(Roles = "Administrator,Manager,Occupational Health Practitioner")]
    public class RisksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RisksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var risks = await _context.Risks
                .Include(r => r.Client)
                .OrderByDescending(r => r.AssessmentDate)
                .ToListAsync();

            return View(risks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var risk = await _context.Risks
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.RiskId == id);

            if (risk == null)
                return NotFound();

            return View(risk);
        }

        public async Task<IActionResult> Create()
        {
            await LoadClientsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Risk risk)
        {
            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(risk.ClientId);
                return View(risk);
            }

            _context.Risks.Add(risk);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Risk assessment successfully created.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var risk = await _context.Risks.FindAsync(id);

            if (risk == null)
                return NotFound();

            await LoadClientsAsync(risk.ClientId);

            return View(risk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Risk risk)
        {
            if (id != risk.RiskId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(risk.ClientId);
                return View(risk);
            }

            _context.Update(risk);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Risk assessment successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var risk = await _context.Risks
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.RiskId == id);

            if (risk == null)
                return NotFound();

            return View(risk);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var risk = await _context.Risks.FindAsync(id);

            if (risk != null)
            {
                _context.Risks.Remove(risk);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Risk assessment successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadClientsAsync(
            int? selectedClientId = null)
        {
            var clients = await _context.Clients
                .Where(c => c.IsActive)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();

            ViewBag.Clients = new SelectList(
                clients,
                "ClientId",
                "CompanyName",
                selectedClientId);
        }
    }
}