using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Identity;
using SamtryggBrfPortal.Infrastructure.Repositories;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;
using SamtryggBrfPortal.Infrastructure.Services;
using SamtryggBrfPortal.Infrastructure.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// For preview purposes, we'll use an in-memory database by default
// You can switch to a real SQL Server database by uncommenting the SQL connection string code
var useInMemoryDb = true; // Set to false to use SQL Server

if (useInMemoryDb)
{
    // Use in-memory database for preview
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("SamtryggBrfPortalDb"));
    
    Console.WriteLine("Using in-memory database for preview. Data will not persist between runs.");
}
else
{
    // Use SQL Server for production/development
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Commented out due to build errors - will be implemented in a future update
// builder.Services.AddScoped<IBrfService, BrfService>();


// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false; // For preview, set to false
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add Authentication and Authorization
builder.Services.AddAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BrfBoard", policy => policy.RequireRole("BrfBoard"));
    options.AddPolicy("PropertyOwner", policy => policy.RequireRole("PropertyOwner"));
    options.AddPolicy("Tenant", policy => policy.RequireRole("Tenant"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

// Add SignalR for real-time notifications
builder.Services.AddSignalR();

// Add MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Seed demo data for preview
if (useInMemoryDb)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("BrfBoard")).Wait();
                roleManager.CreateAsync(new IdentityRole("PropertyOwner")).Wait();
                roleManager.CreateAsync(new IdentityRole("Tenant")).Wait();
            }

            // Create a demo admin user
            if (userManager.FindByEmailAsync("admin@example.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    HasCompletedOnboarding = true
                };

                var result = userManager.CreateAsync(user, "Password123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            Console.WriteLine("Demo user created: admin@example.com / Password123!");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}

app.Run();

// Make the Program class public so it can be accessed from the test project
public partial class Program { }
