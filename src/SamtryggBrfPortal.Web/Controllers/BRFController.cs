using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.Identity;
using SamtryggBrfPortal.Infrastructure.Services.Interfaces;
using SamtryggBrfPortal.Infrastructure.ViewModels;

namespace SamtryggBrfPortal.Web.Controllers
{
    // Temporarily disable authorization for demo purposes
    // [Authorize(Policy = "BrfBoard")]
    public class BRFController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BRFController> _logger;

        public BRFController(
            UserManager<ApplicationUser> userManager,
            ILogger<BRFController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Dashboard()
        {
            try
            {
                // Mock data for demonstration
                var dashboardData = new BrfDashboardViewModel
                {
                    BrfId = Guid.NewGuid(),
                    BrfName = "BRF Solsidan",
                    TotalProperties = 24,
                    AvailableProperties = 3,
                    PendingApplications = 5,
                    RecentApplications = new List<RentalApplicationSummaryViewModel>
                    {
                        new RentalApplicationSummaryViewModel
                        {
                            Id = Guid.NewGuid(),
                            PropertyAddress = "Storgatan 1, Lgh 1201",
                            ApplicantName = "Anna Andersson",
                            ApplicantEmail = "anna.andersson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-2),
                            Status = RentalStatus.Pending
                        },
                        new RentalApplicationSummaryViewModel
                        {
                            Id = Guid.NewGuid(),
                            PropertyAddress = "Storgatan 5, Lgh 1402",
                            ApplicantName = "Erik Eriksson",
                            ApplicantEmail = "erik.eriksson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-5),
                            Status = RentalStatus.Pending
                        }
                    },
                    AvailablePropertiesList = new List<PropertySummaryViewModel>
                    {
                        new PropertySummaryViewModel
                        {
                            Id = Guid.NewGuid(),
                            Address = "Storgatan 1, Lgh 1201",
                            Size = "72 m²",
                            Floor = "2",
                            NumberOfRooms = 3,
                            MonthlyRent = 7500,
                            IsAvailableForRent = true,
                            PrimaryImageUrl = "/images/apartment1.jpg"
                        },
                        new PropertySummaryViewModel
                        {
                            Id = Guid.NewGuid(),
                            Address = "Storgatan 5, Lgh 1402",
                            Size = "85 m²",
                            Floor = "4",
                            NumberOfRooms = 4,
                            MonthlyRent = 8900,
                            IsAvailableForRent = true,
                            PrimaryImageUrl = "/images/apartment2.jpg"
                        }
                    }
                };
                
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data");
                return View("Error");
            }
        }

        public IActionResult Properties()
        {
            try
            {
                // Mock data for demonstration
                var properties = new List<PropertySummaryViewModel>
                {
                    new PropertySummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        Address = "Storgatan 1, Lgh 1201",
                        Size = "72 m²",
                        Floor = "2",
                        MonthlyRent = 7500,
                        IsAvailableForRent = true,
                        PrimaryImageUrl = "/images/apartment1.jpg"
                    },
                    new PropertySummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        Address = "Storgatan 5, Lgh 1402",
                        Size = "85 m²",
                        Floor = "4",
                        MonthlyRent = 8900,
                        IsAvailableForRent = true,
                        PrimaryImageUrl = "/images/apartment2.jpg"
                    },
                    new PropertySummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        Address = "Storgatan 8, Lgh 1101",
                        Size = "65 m²",
                        Floor = "1",
                        MonthlyRent = 6800,
                        IsAvailableForRent = true,
                        PrimaryImageUrl = "/images/apartment3.jpg"
                    }
                };
                
                return View(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving properties");
                return View("Error");
            }
        }

        public IActionResult Applications()
        {
            try
            {
                // Mock data for demonstration
                var applications = new List<RentalApplicationSummaryViewModel>
                {
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 1, Lgh 1201",
                        ApplicantName = "Anna Andersson",
                        ApplicantEmail = "anna.andersson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-2),
                        Status = RentalStatus.Pending
                    },
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 5, Lgh 1402",
                        ApplicantName = "Erik Eriksson",
                        ApplicantEmail = "erik.eriksson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-5),
                        Status = RentalStatus.Pending
                    },
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 8, Lgh 1101",
                        ApplicantName = "Maria Svensson",
                        ApplicantEmail = "maria.svensson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-10),
                        Status = RentalStatus.Approved
                    }
                };
                
                return View(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications");
                return View("Error");
            }
        }

        public IActionResult ApplicationsByStatus(RentalStatus status)
        {
            try
            {
                // Mock data for demonstration - filter by status
                var allApplications = new List<RentalApplicationSummaryViewModel>
                {
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 1, Lgh 1201",
                        ApplicantName = "Anna Andersson",
                        ApplicantEmail = "anna.andersson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-2),
                        Status = RentalStatus.Pending
                    },
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 5, Lgh 1402",
                        ApplicantName = "Erik Eriksson",
                        ApplicantEmail = "erik.eriksson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-5),
                        Status = RentalStatus.Pending
                    },
                    new RentalApplicationSummaryViewModel
                    {
                        Id = Guid.NewGuid(),
                        PropertyAddress = "Storgatan 8, Lgh 1101",
                        ApplicantName = "Maria Svensson",
                        ApplicantEmail = "maria.svensson@example.com",
                            SubmittedAt = DateTime.Now.AddDays(-10),
                        Status = RentalStatus.Approved
                    }
                };
                
                var filteredApplications = allApplications.FindAll(a => a.Status == status);
                return View("Applications", filteredApplications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications with status {Status}", status);
                return View("Error");
            }
        }

        public IActionResult ApplicationDetails(Guid id)
        {
            try
            {
                // Mock data for demonstration
                var application = new RentalApplicationSummaryViewModel
                {
                    Id = id,
                    PropertyAddress = "Storgatan 1, Lgh 1201",
                    ApplicantName = "Anna Andersson",
                    ApplicantEmail = "anna.andersson@example.com",
                    SubmittedAt = DateTime.Now.AddDays(-2),
                    Status = RentalStatus.Pending
                };
                
                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application with ID {ApplicationId}", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateApplicationStatus(Guid id, RentalStatus status)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                
                _logger.LogInformation("Application with ID {ApplicationId} status updated to {Status} by user {UserId}", 
                    id, status, userId);
                
                return RedirectToAction(nameof(ApplicationDetails), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application with ID {ApplicationId} status to {Status}", id, status);
                return View("Error");
            }
        }

        public IActionResult Documents()
        {
            try
            {
                // Mock data for demonstration
                var documents = new List<object>
                {
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stadgar 2024",
                        Type = "PDF",
                        UploadDate = DateTime.Now.AddMonths(-2),
                        Size = "1.2 MB"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Årsredovisning 2023",
                        Type = "PDF",
                        UploadDate = DateTime.Now.AddMonths(-3),
                        Size = "3.5 MB"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Informationsblad Q1 2024",
                        Type = "PDF",
                        UploadDate = DateTime.Now.AddDays(-15),
                        Size = "0.8 MB"
                    }
                };
                
                return View(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents");
                return View("Error");
            }
        }

        public IActionResult Members()
        {
            try
            {
                // Mock data for demonstration
                var members = new List<object>
                {
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Johan Johansson",
                        Role = "Ordförande",
                        Email = "johan.johansson@example.com",
                        Phone = "070-123 45 67"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Lisa Lindberg",
                        Role = "Sekreterare",
                        Email = "lisa.lindberg@example.com",
                        Phone = "070-234 56 78"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Anders Andersson",
                        Role = "Kassör",
                        Email = "anders.andersson@example.com",
                        Phone = "070-345 67 89"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        Name = "Karin Karlsson",
                        Role = "Ledamot",
                        Email = "karin.karlsson@example.com",
                        Phone = "070-456 78 90"
                    }
                };
                
                return View(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving board members");
                return View("Error");
            }
        }
    }
}
