using Microsoft.AspNetCore.Mvc;
using TOHMS.API.Controllers;
using TOHMS.Models;
using TOHMS.Tests.Helpers;

namespace TOHMS.Tests.Controllers
{
    public class ClientsApiControllerTests
    {
        [Fact]
        public async Task GetClients_ReturnsOkResult()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            context.Clients.Add(
                new Client
                {
                    CompanyName =
                        "Thahla Test Client"
                });

            await context.SaveChangesAsync();

            var controller =
                new ClientsApiController(context);

            // Act

            var result =
                await controller.GetClients();

            // Assert

            Assert.IsType<OkObjectResult>(
                result.Result);
        }


        [Fact]
        public async Task GetClient_ExistingClient_ReturnsOk()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var client = new Client
            {
                CompanyName =
                    "Existing Company"
            };

            context.Clients.Add(client);

            await context.SaveChangesAsync();

            var controller =
                new ClientsApiController(context);

            // Act

            var result =
                await controller.GetClient(
                    client.ClientId);

            // Assert

            var okResult =
                Assert.IsType<OkObjectResult>(
                    result.Result);

            var returnedClient =
                Assert.IsType<Client>(
                    okResult.Value);

            Assert.Equal(
                "Existing Company",
                returnedClient.CompanyName);
        }


        [Fact]
        public async Task GetClient_InvalidId_ReturnsNotFound()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var controller =
                new ClientsApiController(context);

            // Act

            var result =
                await controller.GetClient(999);

            // Assert

            Assert.IsType<NotFoundObjectResult>(
                result.Result);
        }


        [Fact]
        public async Task CreateClient_AddsClientToDatabase()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var controller =
                new ClientsApiController(context);

            var client = new Client
            {
                CompanyName =
                    "New Occupational Health Client",

                ContactPerson =
                    "Test Manager",

                Email =
                    "manager@example.com",

                PhoneNumber =
                    "0111234567",

                Industry =
                    "Construction"
            };

            // Act

            var result =
                await controller.CreateClient(client);

            // Assert

            Assert.IsType<
                CreatedAtActionResult>(
                result.Result);

            Assert.Single(
                context.Clients);

            Assert.Equal(
                "New Occupational Health Client",
                context.Clients.First().CompanyName);
        }


        [Fact]
        public async Task UpdateClient_ExistingClient_UpdatesDatabase()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var existingClient = new Client
            {
                CompanyName =
                    "Old Company Name"
            };

            context.Clients.Add(existingClient);

            await context.SaveChangesAsync();

            var controller =
                new ClientsApiController(context);

            var updatedClient = new Client
            {
                ClientId =
                    existingClient.ClientId,

                CompanyName =
                    "Updated Company Name",

                ContactPerson =
                    "Updated Person",

                Email =
                    "updated@example.com",

                Industry =
                    "Manufacturing",

                IsActive = true
            };

            // Act

            var result =
                await controller.UpdateClient(
                    existingClient.ClientId,
                    updatedClient);

            // Assert

            Assert.IsType<NoContentResult>(
                result);

            var databaseClient =
                await context.Clients.FindAsync(
                    existingClient.ClientId);

            Assert.NotNull(databaseClient);

            Assert.Equal(
                "Updated Company Name",
                databaseClient.CompanyName);
        }


        [Fact]
        public async Task UpdateClient_MismatchedId_ReturnsBadRequest()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var controller =
                new ClientsApiController(context);

            var client = new Client
            {
                ClientId = 5,
                CompanyName = "Test Company"
            };

            // Act

            var result =
                await controller.UpdateClient(
                    10,
                    client);

            // Assert

            Assert.IsType<BadRequestObjectResult>(
                result);
        }


        [Fact]
        public async Task DeleteClient_ExistingClient_RemovesClient()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var client = new Client
            {
                CompanyName =
                    "Client To Delete"
            };

            context.Clients.Add(client);

            await context.SaveChangesAsync();

            var controller =
                new ClientsApiController(context);

            // Act

            var result =
                await controller.DeleteClient(
                    client.ClientId);

            // Assert

            Assert.IsType<NoContentResult>(
                result);

            Assert.Empty(
                context.Clients);
        }


        [Fact]
        public async Task DeleteClient_InvalidId_ReturnsNotFound()
        {
            // Arrange

            using var context =
                TestDbContextFactory.CreateContext();

            var controller =
                new ClientsApiController(context);

            // Act

            var result =
                await controller.DeleteClient(999);

            // Assert

            Assert.IsType<NotFoundObjectResult>(
                result);
        }
    }
}