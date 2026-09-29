using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;


namespace TOHMS.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Invoice list
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Client)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return View(invoices);
        }

        // Invoice details
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.InvoiceItems)
                    .ThenInclude(ii => ii.Service)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // Create invoice
        public async Task<IActionResult> Create()
        {
            await LoadClientsAsync();

            var invoice = new Invoice
            {
                InvoiceNumber = GenerateInvoiceNumber(),
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(30),
                Status = "Unpaid"
            };

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(invoice.ClientId);
                return View(invoice);
            }

            invoice.TotalAmount = 0;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Invoice created. You can now add services.";

            return RedirectToAction(
                nameof(Details),
                new { id = invoice.InvoiceId });
        }

        // Edit invoice header
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
                return NotFound();

            await LoadClientsAsync(invoice.ClientId);

            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Invoice invoice)
        {
            if (id != invoice.InvoiceId)
                return NotFound();

            var existingInvoice =
                await _context.Invoices.FindAsync(id);

            if (existingInvoice == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadClientsAsync(invoice.ClientId);
                return View(invoice);
            }

            existingInvoice.ClientId = invoice.ClientId;
            existingInvoice.InvoiceNumber = invoice.InvoiceNumber;
            existingInvoice.InvoiceDate = invoice.InvoiceDate;
            existingInvoice.DueDate = invoice.DueDate;
            existingInvoice.Status = invoice.Status;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Invoice successfully updated.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        // Add invoice item
        public async Task<IActionResult> AddItem(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
                return NotFound();

            await LoadServicesAsync();

            ViewBag.Invoice = invoice;

            return View(new InvoiceItem
            {
                InvoiceId = id,
                Quantity = 1
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(
            InvoiceItem invoiceItem)
        {
            var invoice =
                await _context.Invoices.FindAsync(
                    invoiceItem.InvoiceId);

            if (invoice == null)
                return NotFound();

            var service =
                await _context.Services.FindAsync(
                    invoiceItem.ServiceId);

            if (service == null)
            {
                ModelState.AddModelError(
                    "ServiceId",
                    "Please select a valid service.");
            }

            if (!ModelState.IsValid)
            {
                await LoadServicesAsync(
                    invoiceItem.ServiceId);

                ViewBag.Invoice = invoice;

                return View(invoiceItem);
            }

            invoiceItem.UnitPrice = service!.Price;

            invoiceItem.TotalPrice =
                invoiceItem.Quantity *
                invoiceItem.UnitPrice;

            _context.InvoiceItems.Add(invoiceItem);

            await _context.SaveChangesAsync();

            await RecalculateInvoiceTotalAsync(
                invoiceItem.InvoiceId);

            TempData["SuccessMessage"] =
                "Service successfully added to invoice.";

            return RedirectToAction(
                nameof(Details),
                new { id = invoiceItem.InvoiceId });
        }

        // Remove invoice item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var item =
                await _context.InvoiceItems.FindAsync(id);

            if (item == null)
                return NotFound();

            var invoiceId = item.InvoiceId;

            _context.InvoiceItems.Remove(item);

            await _context.SaveChangesAsync();

            await RecalculateInvoiceTotalAsync(invoiceId);

            TempData["SuccessMessage"] =
                "Service removed from invoice.";

            return RedirectToAction(
                nameof(Details),
                new { id = invoiceId });
        }

        // Delete invoice
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice =
                await _context.Invoices.FindAsync(id);

            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] =
                "Invoice successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task RecalculateInvoiceTotalAsync(
            int invoiceId)
        {
            var invoice =
                await _context.Invoices.FindAsync(invoiceId);

            if (invoice == null)
                return;

            invoice.TotalAmount =
                await _context.InvoiceItems
                    .Where(i => i.InvoiceId == invoiceId)
                    .SumAsync(i => i.TotalPrice);

            await _context.SaveChangesAsync();
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

        private async Task LoadServicesAsync(
            int? selectedServiceId = null)
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.ServiceName)
                .ToListAsync();

            ViewBag.Services = new SelectList(
                services,
                "ServiceId",
                "ServiceName",
                selectedServiceId);
        }

        private string GenerateInvoiceNumber()
        {
            return "INV-" +
                   DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}