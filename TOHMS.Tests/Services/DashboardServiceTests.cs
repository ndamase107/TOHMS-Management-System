using TOHMS.Models;
using TOHMS.Services;
using TOHMS.Tests.Helpers;

namespace TOHMS.Tests.Services
{
    public class DashboardServiceTests
    {
        [Fact]
        public async Task GetDashboardAsync_ReturnsCorrectClientCount()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            context.Clients.AddRange(
                new Client
                {
                    CompanyName = "Company A"
                },
                new Client
                {
                    CompanyName = "Company B"
                },
                new Client
                {
                    CompanyName = "Company C"
                });

            await context.SaveChangesAsync();

            var service =
                new DashboardService(context);

            // Act

            var result =
                await service.GetDashboardAsync();

            // Assert

            Assert.Equal(
                3,
                result.TotalClients);
        }


        [Fact]
        public async Task GetDashboardAsync_CountsUnpaidInvoicesAsOutstanding()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var client = new Client
            {
                CompanyName = "Test Company"
            };

            context.Clients.Add(client);

            await context.SaveChangesAsync();

            context.Invoices.AddRange(
                new Invoice
                {
                    ClientId = client.ClientId,
                    InvoiceNumber = "INV-001",
                    InvoiceDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(30),
                    Status = "Unpaid",
                    TotalAmount = 1000
                },
                new Invoice
                {
                    ClientId = client.ClientId,
                    InvoiceNumber = "INV-002",
                    InvoiceDate = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(30),
                    Status = "Paid",
                    TotalAmount = 500
                });

            await context.SaveChangesAsync();

            var service =
                new DashboardService(context);

            // Act

            var result =
                await service.GetDashboardAsync();

            // Assert

            Assert.Equal(
                1,
                result.OutstandingInvoices);
        }


        [Fact]
        public async Task GetDashboardAsync_CountsActiveRisks()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var client = new Client
            {
                CompanyName = "Mining Company"
            };

            context.Clients.Add(client);

            await context.SaveChangesAsync();

            context.Risks.AddRange(
                new Risk
                {
                    ClientId = client.ClientId,
                    Hazard = "Noise Exposure",
                    RiskLevel = "High",
                    AssessmentDate = DateTime.Today,
                    IsActive = true
                },
                new Risk
                {
                    ClientId = client.ClientId,
                    Hazard = "Old Hazard",
                    RiskLevel = "Low",
                    AssessmentDate = DateTime.Today,
                    IsActive = false
                });

            await context.SaveChangesAsync();

            var service =
                new DashboardService(context);

            // Act

            var result =
                await service.GetDashboardAsync();

            // Assert

            Assert.Equal(
                1,
                result.OpenRisks);
        }


        [Fact]
        public async Task GetDashboardAsync_CountsPendingExaminations()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var client = new Client
            {
                CompanyName = "Construction Company"
            };

            context.Clients.Add(client);

            await context.SaveChangesAsync();

            var employee = new Employee
            {
                ClientId = client.ClientId,
                EmployeeNumber = "EMP001",
                FirstName = "Test",
                LastName = "Employee",
                DateOfBirth = new DateTime(1990, 1, 1),
                EmploymentDate = DateTime.Today
            };

            context.Employees.Add(employee);

            await context.SaveChangesAsync();

            context.Examinations.AddRange(
                new Examination
                {
                    EmployeeId = employee.EmployeeId,
                    ExaminationType =
                        "Medical Surveillance",
                    ExaminationDate = DateTime.Today,
                    FitnessStatus = "Pending"
                },
                new Examination
                {
                    EmployeeId = employee.EmployeeId,
                    ExaminationType =
                        "Periodic Medical",
                    ExaminationDate = DateTime.Today,
                    FitnessStatus = "Fit"
                });

            await context.SaveChangesAsync();

            var service =
                new DashboardService(context);

            // Act

            var result =
                await service.GetDashboardAsync();

            // Assert

            Assert.Equal(
                1,
                result.PendingExaminations);
        }
    }
}