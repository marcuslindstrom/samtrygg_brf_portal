using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SamtryggBrfPortal.Core.Entities;

namespace SamtryggBrfPortal.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Property entities
    /// </summary>
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        /// <summary>
        /// Gets properties by BRF association ID
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of properties for the specified BRF association</returns>
        Task<IReadOnlyList<Property>> GetByBrfIdAsync(Guid brfId);

        /// <summary>
        /// Gets properties by owner user ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>A list of properties owned by the specified user</returns>
        Task<IReadOnlyList<Property>> GetByOwnerIdAsync(string userId);

        /// <summary>
        /// Gets a property with its images included
        /// </summary>
        /// <param name="id">The property ID</param>
        /// <returns>The property with images if found, null otherwise</returns>
        Task<Property> GetWithImagesAsync(Guid id);

        /// <summary>
        /// Gets a property with its rental applications included
        /// </summary>
        /// <param name="id">The property ID</param>
        /// <returns>The property with rental applications if found, null otherwise</returns>
        Task<Property> GetWithApplicationsAsync(Guid id);

        /// <summary>
        /// Gets a property with all related entities included
        /// </summary>
        /// <param name="id">The property ID</param>
        /// <returns>The property with all related entities if found, null otherwise</returns>
        Task<Property> GetCompleteAsync(Guid id);

        /// <summary>
        /// Gets available properties for a BRF association
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of available properties for the specified BRF association</returns>
        Task<IReadOnlyList<Property>> GetAvailableByBrfIdAsync(Guid brfId);

        /// <summary>
        /// Gets properties with active rental applications
        /// </summary>
        /// <param name="brfId">The BRF association ID</param>
        /// <returns>A list of properties with active rental applications</returns>
        Task<IReadOnlyList<Property>> GetWithActiveApplicationsAsync(Guid brfId);
    }
}
