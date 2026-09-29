using Microsoft.AspNetCore.Mvc;
using TOHMS.Models;
using TOHMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;



namespace TOHMS.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: /Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();
            return View(clients);
        }

        // GET: /Clients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: /Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            await _clientService.AddAsync(client);

            TempData["SuccessMessage"] = "Client successfully added.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: /Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(client);
            }

            await _clientService.UpdateAsync(client);

            TempData["SuccessMessage"] = "Client successfully updated.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Clients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: /Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteAsync(id);

            TempData["SuccessMessage"] = "Client successfully deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}