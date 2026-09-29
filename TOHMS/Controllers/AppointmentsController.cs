using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using TOHMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;



namespace TOHMS.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly ApplicationDbContext _context;

        public AppointmentsController(
            IRepository<Appointment> appointmentRepository,
            ApplicationDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Employee)
                .ThenInclude(e => e.Client)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(appointment.EmployeeId);
                return View(appointment);
            }

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment successfully scheduled.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            await LoadEmployeesAsync(appointment.EmployeeId);

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Appointment appointment)
        {
            if (id != appointment.AppointmentId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(appointment.EmployeeId);
                return View(appointment);
            }

            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .ThenInclude(e => e.Client)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment != null)
            {
                _appointmentRepository.Delete(appointment);
                await _appointmentRepository.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Appointment successfully deleted.";

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