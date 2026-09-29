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
    public class EmployeesController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ApplicationDbContext _context;

        public EmployeesController(
            IRepository<Employee> employeeRepository,
            ApplicationDbContext context)
        {
            _employeeRepository = employeeRepository;
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Client)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            await LoadClientsAsync();

            return View();
        }

        // POST: Employees/Create
        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Client is a navigation property.
            // The form supplies ClientId, not a complete Client object.
            ModelState.Remove("Client");

            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(employee.ClientId);
                return View(employee);
            }

            employee.CreatedDate = DateTime.UtcNow;

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee successfully added.";

            return RedirectToAction(nameof(Index));
        }
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            await LoadClientsAsync(employee.ClientId);

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            ModelState.Remove("Client");

            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(employee.ClientId);
                return View(employee);
            }

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee successfully updated.";

            return RedirectToAction(nameof(Index));
        }
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Client)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee != null)
            {
                _employeeRepository.Delete(employee);
                await _employeeRepository.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Employee successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadClientsAsync(int? selectedClientId = null)
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