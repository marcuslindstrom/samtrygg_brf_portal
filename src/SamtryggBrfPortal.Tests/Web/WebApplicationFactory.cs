using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Web;

namespace SamtryggBrfPortal.Tests.Web
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Find the service descriptor for the DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    // Remove the registered DbContext
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data
                        SeedTestData(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void SeedTestData(ApplicationDbContext dbContext)
        {
            // Add test data here
            // For example, you could add test BRF associations, properties, etc.
            // This is similar to what we did in the DatabaseFixture

            // Save changes to the database
            dbContext.SaveChanges();
        }
    }
}
