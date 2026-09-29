using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Services.Interfaces;
using TOHMS.ViewModels;

namespace TOHMS.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            var today = DateTime.Today;

            return new DashboardViewModel
            {
                TotalClients =
                    await _context.Clients.CountAsync(),

                TotalEmployees =
                    await _context.Employees.CountAsync(),

                TotalAppointments =
                    await _context.Appointments.CountAsync(),

                TotalCertificates =
                    await _context.Certificates.CountAsync(),

                TotalRisks =
                    await _context.Risks.CountAsync(),

                TotalInvoices =
                    await _context.Invoices.CountAsync(),

                UpcomingAppointments =
                    await _context.Appointments.CountAsync(a =>
                        a.AppointmentDate >= today &&
                        a.Status != "Cancelled" &&
                        a.Status != "Completed"),

                PendingExaminations =
                    await _context.Examinations.CountAsync(e =>
                        e.FitnessStatus == "Pending"),

                ActiveCertificates =
                    await _context.Certificates.CountAsync(c =>
                        c.Status == "Active" &&
                        (!c.ExpiryDate.HasValue ||
                         c.ExpiryDate.Value >= today)),

                OpenRisks =
                    await _context.Risks.CountAsync(r =>
                        r.IsActive),

                OutstandingInvoices =
                    await _context.Invoices.CountAsync(i =>
                        i.Status == "Unpaid" ||
                        i.Status == "Overdue")
            };
        }
    }
}