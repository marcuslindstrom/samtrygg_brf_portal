using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.ViewModels;

namespace SamtryggBrfPortal.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Service interface for BRF operations
    /// </summary>
    public interface IBrfService
    {
        /// <summary>
        /// Gets dashboard data for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>Dashboard data</returns>
        Task<BrfDashboardViewModel> GetDashboardDataAsync(Guid brfId);

        /// <summary>
        /// Gets dashboard data for a BRF association by user ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>Dashboard data</returns>
        Task<BrfDashboardViewModel> GetDashboardDataByUserIdAsync(string userId);

        /// <summary>
        /// Gets properties for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of properties</returns>
        Task<List<PropertySummaryViewModel>> GetPropertiesAsync(Guid brfId);

        /// <summary>
        /// Gets rental applications for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of rental applications</returns>
        Task<List<RentalApplicationSummaryViewModel>> GetApplicationsAsync(Guid brfId);

        /// <summary>
        /// Gets rental applications for a BRF association by status
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <param name="status">The rental application status</param>
        /// <returns>A list of rental applications</returns>
        Task<List<RentalApplicationSummaryViewModel>> GetApplicationsByStatusAsync(Guid brfId, RentalStatus status);

        /// <summary>
        /// Gets a rental application by ID
        /// </summary>
        /// <param name="applicationId">The rental application ID</param>
        /// <returns>The rental application</returns>
        Task<RentalApplication> GetApplicationByIdAsync(Guid applicationId);

        /// <summary>
        /// Updates the status of a rental application
        /// </summary>
        /// <param name="applicationId">The rental application ID</param>
        /// <param name="status">The new status</param>
        /// <param name="updatedByUserId">The ID of the user who updated the status</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateApplicationStatusAsync(Guid applicationId, RentalStatus status, string updatedByUserId);

        /// <summary>
        /// Gets board members for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of board members</returns>
        Task<List<BrfBoardMember>> GetBoardMembersAsync(Guid brfId);

        /// <summary>
        /// Gets documents for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of documents</returns>
        Task<List<BrfDocument>> GetDocumentsAsync(Guid brfId);
    }
}
