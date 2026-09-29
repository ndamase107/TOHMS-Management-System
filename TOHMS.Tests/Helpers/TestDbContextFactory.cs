using Microsoft.EntityFrameworkCore;
using TOHMS.Data;

namespace TOHMS.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateContext()
        {
            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(
                        databaseName: Guid.NewGuid().ToString())
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}