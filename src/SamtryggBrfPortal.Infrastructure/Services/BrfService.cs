using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;
using SamtryggBrfPortal.Infrastructure.Services.Interfaces;
using SamtryggBrfPortal.Infrastructure.ViewModels;

namespace SamtryggBrfPortal.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for BRF operations
    /// </summary>
    public class BrfService : IBrfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrfService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="logger">Logger</param>
        public BrfService(IUnitOfWork unitOfWork, ILogger<BrfService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<BrfDashboardViewModel> GetDashboardDataAsync(Guid brfId)
        {
            try
            {
                var brf = await _unitOfWork.BrfAssociations.GetWithPropertiesAsync(brfId);
                if (brf == null)
                {
                    _logger.LogWarning("BRF association with ID {BrfId} not found", brfId);
                    return null;
                }

                var pendingApplications = await _unitOfWork.RentalApplications.GetPendingByBrfIdAsync(brfId);
                var allApplications = await _unitOfWork.RentalApplications.GetByBrfIdAsync(brfId);
                var boardMembers = await _unitOfWork.BrfAssociations.GetWithBoardMembersAsync(brfId);
                var availableProperties = await _unitOfWork.Properties.GetAvailableByBrfIdAsync(brfId);

                var dashboard = new BrfDashboardViewModel
                {
                    BrfId = brf.Id,
                    BrfName = brf.Name,
                    TotalProperties = brf.Properties?.Count ?? 0,
                    AvailableProperties = availableProperties.Count,
                    PendingApplications = pendingApplications.Count,
                    ApprovedApplicationsCount = allApplications.Count(a => a.Status == RentalStatus.Approved),
                    RejectedApplicationsCount = allApplications.Count(a => a.Status == RentalStatus.Rejected),
                    BoardMemberCount = boardMembers?.BoardMembers?.Count ?? 0,
                    RecentApplications = pendingApplications.Take(5).Select(a => new RentalApplicationSummaryViewModel
                    {
                        Id = a.Id,
                        PropertyId = a.PropertyId,
                        PropertyAddress = a.Property?.Address,
                        ApplicantName = $"{a.ApplicantFirstName} {a.ApplicantLastName}",
                        ApplicantEmail = a.ApplicantEmail,
                        Status = a.Status,
                        SubmittedAt = a.CreatedAt,
                        LastUpdated = a.ModifiedAt ?? a.CreatedAt,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        HasBackgroundCheck = a.BackgroundCheck != null,
                        BackgroundCheckStatus = a.BackgroundCheck?.Status,
                        UnreadMessagesCount = a.Messages?.Count(m => !m.IsRead && m.RecipientUserId != a.ApplicantUserId) ?? 0
                    }).ToList(),
                    AvailablePropertiesList = availableProperties.Take(5).Select(p => new PropertySummaryViewModel
                    {
                        Id = p.Id,
                        Address = p.Address,
                        Size = p.Size,
                        NumberOfRooms = p.NumberOfRooms,
                        Floor = p.Floor,
                        MonthlyRent = p.MonthlyRent,
                        IsAvailableForRent = p.IsAvailableForRent,
                        LastUpdated = p.ModifiedAt ?? p.CreatedAt,
                        PrimaryImageUrl = p.Images?.FirstOrDefault()?.ImageUrl,
                        ActiveApplicationsCount = p.RentalApplications?.Count(a => a.Status == RentalStatus.Pending) ?? 0
                    }).ToList()
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data for BRF association with ID {BrfId}", brfId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<BrfDashboardViewModel> GetDashboardDataByUserIdAsync(string userId)
        {
            try
            {
                var brf = await _unitOfWork.BrfAssociations.GetByUserIdAsync(userId);
                if (brf == null)
                {
                    _logger.LogWarning("BRF association not found for user with ID {UserId}", userId);
                    return null;
                }

                return await GetDashboardDataAsync(brf.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data for user with ID {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<PropertySummaryViewModel>> GetPropertiesAsync(Guid brfId)
        {
            try
            {
                var properties = await _unitOfWork.Properties.GetByBrfIdAsync(brfId);
                
                return properties.Select(p => new PropertySummaryViewModel
                {
                    Id = p.Id,
                    Address = p.Address,
                    Size = p.Size,
                    NumberOfRooms = p.NumberOfRooms,
                    Floor = p.Floor,
                    MonthlyRent = p.MonthlyRent,
                    IsAvailableForRent = p.IsAvailableForRent,
                    LastUpdated = p.ModifiedAt ?? p.CreatedAt,
                    PrimaryImageUrl = p.Images?.FirstOrDefault()?.ImageUrl,
                    ActiveApplicationsCount = p.RentalApplications?.Count(a => a.Status == RentalStatus.Pending) ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting properties for BRF association with ID {BrfId}", brfId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<RentalApplicationSummaryViewModel>> GetApplicationsAsync(Guid brfId)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByBrfIdAsync(brfId);
                
                return applications.Select(a => new RentalApplicationSummaryViewModel
                {
                    Id = a.Id,
                    PropertyId = a.PropertyId,
                    PropertyAddress = a.Property?.Address,
                    ApplicantName = $"{a.ApplicantFirstName} {a.ApplicantLastName}",
                    ApplicantEmail = a.ApplicantEmail,
                    Status = a.Status,
                    SubmittedAt = a.CreatedAt,
                    LastUpdated = a.ModifiedAt ?? a.CreatedAt,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    HasBackgroundCheck = a.BackgroundCheck != null,
                    BackgroundCheckStatus = a.BackgroundCheck?.Status,
                    UnreadMessagesCount = a.Messages?.Count(m => !m.IsRead && m.RecipientUserId != a.ApplicantUserId) ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting applications for BRF association with ID {BrfId}", brfId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<RentalApplicationSummaryViewModel>> GetApplicationsByStatusAsync(Guid brfId, RentalStatus status)
        {
            try
            {
                var applications = await _unitOfWork.RentalApplications.GetByBrfIdAsync(brfId);
                var filteredApplications = applications.Where(a => a.Status == status).ToList();
                
                return filteredApplications.Select(a => new RentalApplicationSummaryViewModel
                {
                    Id = a.Id,
                    PropertyId = a.PropertyId,
                    PropertyAddress = a.Property?.Address,
                    ApplicantName = $"{a.ApplicantFirstName} {a.ApplicantLastName}",
                    ApplicantEmail = a.ApplicantEmail,
                    Status = a.Status,
                    SubmittedAt = a.CreatedAt,
                    LastUpdated = a.ModifiedAt ?? a.CreatedAt,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    HasBackgroundCheck = a.BackgroundCheck != null,
                    BackgroundCheckStatus = a.BackgroundCheck?.Status,
                    UnreadMessagesCount = a.Messages?.Count(m => !m.IsRead && m.RecipientUserId != a.ApplicantUserId) ?? 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting applications with status {Status} for BRF association with ID {BrfId}", status, brfId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<RentalApplication> GetApplicationByIdAsync(Guid applicationId)
        {
            try
            {
                return await _unitOfWork.RentalApplications.GetCompleteAsync(applicationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application with ID {ApplicationId}", applicationId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateApplicationStatusAsync(Guid applicationId, RentalStatus status, string updatedByUserId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                
                await _unitOfWork.RentalApplications.UpdateStatusAsync(applicationId, status, updatedByUserId);
                await _unitOfWork.CompleteAsync();
                
                await _unitOfWork.CommitTransactionAsync();
                
                _logger.LogInformation("Application with ID {ApplicationId} status updated to {Status} by user {UserId}", 
                    applicationId, status, updatedByUserId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating application with ID {ApplicationId} status to {Status}", applicationId, status);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<BrfBoardMember>> GetBoardMembersAsync(Guid brfId)
        {
            try
            {
                var brf = await _unitOfWork.BrfAssociations.GetWithBoardMembersAsync(brfId);
                return brf?.BoardMembers?.ToList() ?? new List<BrfBoardMember>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting board members for BRF association with ID {BrfId}", brfId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<BrfDocument>> GetDocumentsAsync(Guid brfId)
        {
            try
            {
                var brf = await _unitOfWork.BrfAssociations.GetCompleteAsync(brfId);
                return brf?.Documents?.ToList() ?? new List<BrfDocument>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting documents for BRF association with ID {BrfId}", brfId);
                throw;
            }
        }
    }
}
