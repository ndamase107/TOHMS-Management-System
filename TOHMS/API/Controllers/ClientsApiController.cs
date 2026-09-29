using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;

namespace TOHMS.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    [Authorize]
    public class ClientsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsApiController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET: api/clients
        // Returns all clients
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients
                .AsNoTracking()
                .OrderBy(c => c.CompanyName)
                .ToListAsync();

            return Ok(clients);
        }

        // =====================================================
        // GET: api/clients/5
        // Returns one client
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.ClientId == id);

            if (client == null)
            {
                return NotFound(new
                {
                    message =
                        $"Client with ID {id} was not found."
                });
            }

            return Ok(client);
        }

        // =====================================================
        // POST: api/clients
        // Creates a new client
        // =====================================================

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager,Reception")]
        public async Task<ActionResult<Client>> CreateClient(
            Client client)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            client.ClientId = 0;
            client.CreatedDate = DateTime.UtcNow;

            _context.Clients.Add(client);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetClient),
                new { id = client.ClientId },
                client);
        }

        // =====================================================
        // PUT: api/clients/5
        // Updates an existing client
        // =====================================================

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator,Manager,Reception")]
        public async Task<IActionResult> UpdateClient(
            int id,
            Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest(new
                {
                    message =
                        "The URL client ID does not match the client ID in the request."
                });
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existingClient =
                await _context.Clients.FindAsync(id);

            if (existingClient == null)
            {
                return NotFound(new
                {
                    message =
                        $"Client with ID {id} was not found."
                });
            }

            existingClient.CompanyName =
                client.CompanyName;

            existingClient.ContactPerson =
                client.ContactPerson;

            existingClient.PhoneNumber =
                client.PhoneNumber;

            existingClient.Email =
                client.Email;

            existingClient.Address =
                client.Address;

            existingClient.Industry =
                client.Industry;

            existingClient.IsActive =
                client.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // =====================================================
        // DELETE: api/clients/5
        // Deletes a client
        // =====================================================

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client =
                await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound(new
                {
                    message =
                        $"Client with ID {id} was not found."
                });
            }

            var hasEmployees =
                await _context.Employees
                    .AnyAsync(e =>
                        e.ClientId == id);

            var hasInvoices =
                await _context.Invoices
                    .AnyAsync(i =>
                        i.ClientId == id);

            if (hasEmployees || hasInvoices)
            {
                return Conflict(new
                {
                    message =
                        "This client cannot be deleted because it has related employees or invoices."
                });
            }

            _context.Clients.Remove(client);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}