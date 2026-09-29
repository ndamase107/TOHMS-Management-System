using Microsoft.AspNetCore.Identity;
using TOHMS.Models;

namespace TOHMS.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            string[] roles =
            {
                "Administrator",
                "Manager",
                "Occupational Health Practitioner",
                "Reception",
                "Client Representative"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            const string adminEmail =
                "admin@tohms.co.za";

            var admin =
                await userManager.FindByEmailAsync(
                    adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    JobTitle = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result =
                    await userManager.CreateAsync(
                        admin,
                        "Admin@12345");

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(e =>
                            e.Description));

                    throw new Exception(
                        $"Unable to create administrator: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(
                    admin,
                    "Administrator"))
            {
                await userManager.AddToRoleAsync(
                    admin,
                    "Administrator");
            }
        }
    }
}