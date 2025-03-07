using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;

namespace SamtryggBrfPortal.Tests.Infrastructure
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }

        private readonly ServiceProvider _serviceProvider;

        public DatabaseFixture()
        {
            var services = new ServiceCollection();

            // Add DbContext using an in-memory database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid().ToString()));

            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            _serviceProvider = services.BuildServiceProvider();

            // Create a new DbContext instance
            DbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure the database is created
            DbContext.Database.EnsureCreated();

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Create roles
            if (!RoleManager.RoleExistsAsync("Admin").Result)
            {
                RoleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                RoleManager.CreateAsync(new IdentityRole("BrfBoard")).Wait();
                RoleManager.CreateAsync(new IdentityRole("PropertyOwner")).Wait();
                RoleManager.CreateAsync(new IdentityRole("Tenant")).Wait();
            }

            // Create a test admin user
            if (UserManager.FindByEmailAsync("test@example.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "test@example.com",
                    Email = "test@example.com",
                    EmailConfirmed = true,
                    FirstName = "Test",
                    LastName = "User",
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    HasCompletedOnboarding = true
                };

                var result = UserManager.CreateAsync(user, "Password123!").Result;
                if (result.Succeeded)
                {
                    UserManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            // Add more test data as needed for your tests
            // For example, you could add test BRF associations, properties, etc.

            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
