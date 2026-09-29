using System.ComponentModel.DataAnnotations;
using TOHMS.Models;

namespace TOHMS.Tests.Models
{
    public class ClientValidationTests
    {
        private static List<ValidationResult>
            ValidateModel(object model)
        {
            var results =
                new List<ValidationResult>();

            var context =
                new ValidationContext(model);

            Validator.TryValidateObject(
                model,
                context,
                results,
                validateAllProperties: true);

            return results;
        }


        [Fact]
        public void Client_WithoutCompanyName_FailsValidation()
        {
            // Arrange

            var client = new Client
            {
                CompanyName = "",
                Email = "test@example.com"
            };

            // Act

            var results =
                ValidateModel(client);

            // Assert

            Assert.Contains(
                results,
                r => r.MemberNames.Contains(
                    nameof(Client.CompanyName)));
        }


        [Fact]
        public void Client_WithInvalidEmail_FailsValidation()
        {
            // Arrange

            var client = new Client
            {
                CompanyName =
                    "Test Company",

                Email =
                    "this-is-not-an-email"
            };

            // Act

            var results =
                ValidateModel(client);

            // Assert

            Assert.Contains(
                results,
                r => r.MemberNames.Contains(
                    nameof(Client.Email)));
        }


        [Fact]
        public void Client_WithValidInformation_PassesValidation()
        {
            // Arrange

            var client = new Client
            {
                CompanyName =
                    "Valid Company",

                ContactPerson =
                    "Test Person",

                PhoneNumber =
                    "0111234567",

                Email =
                    "test@example.com",

                Industry =
                    "Manufacturing"
            };

            // Act

            var results =
                ValidateModel(client);

            // Assert

            Assert.Empty(results);
        }
    }
}