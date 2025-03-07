using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;

namespace SamtryggBrfPortal.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for RentalApplication entities
    /// </summary>
    public interface IRentalApplicationRepository : IGenericRepository<RentalApplication>
    {
        /// <summary>
        /// Gets rental applications by property ID
        /// </summary>
        /// <param name="propertyId">The property ID</param>
        /// <returns>A list of rental applications for the specified property</returns>
        Task<IReadOnlyList<RentalApplication>> GetByPropertyIdAsync(Guid propertyId);

        /// <summary>
        /// Gets rental applications by BRF association ID
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of rental applications for the specified BRF association</returns>
        Task<IReadOnlyList<RentalApplication>> GetByBrfIdAsync(Guid brfId);

        /// <summary>
        /// Gets rental applications by applicant user ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>A list of rental applications for the specified user</returns>
        Task<IReadOnlyList<RentalApplication>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Gets rental applications by status
        /// </summary>
        /// <param name="status">The rental application status</param>
        /// <returns>A list of rental applications with the specified status</returns>
        Task<IReadOnlyList<RentalApplication>> GetByStatusAsync(RentalStatus status);

        /// <summary>
        /// Gets pending rental applications for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of pending rental applications for the specified BRF association</returns>
        Task<IReadOnlyList<RentalApplication>> GetPendingByBrfIdAsync(Guid brfId);

        /// <summary>
        /// Gets a rental application with all related entities included
        /// </summary>
        /// <param name="id">The rental application ID</param>
        /// <returns>The rental application with all related entities if found, null otherwise</returns>
        Task<RentalApplication> GetCompleteAsync(Guid id);

        /// <summary>
        /// Updates the status of a rental application
        /// </summary>
        /// <param name="id">The rental application ID</param>
        /// <param name="status">The new status</param>
        /// <param name="updatedByUserId">The ID of the user who updated the status</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateStatusAsync(Guid id, RentalStatus status, string updatedByUserId);
    }
}
