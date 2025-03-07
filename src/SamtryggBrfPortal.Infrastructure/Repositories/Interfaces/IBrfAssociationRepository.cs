using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SamtryggBrfPortal.Core.Entities;

namespace SamtryggBrfPortal.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for BrfAssociation entities
    /// </summary>
    public interface IBrfAssociationRepository : IGenericRepository<BrfAssociation>
    {
        /// <summary>
        /// Gets a BRF association by its organization number
        /// </summary>
        /// <param name="orgNumber">The organization number</param>
        /// <returns>The BRF association if found, null otherwise</returns>
        Task<BrfAssociation> GetByOrgNumberAsync(string orgNumber);

        /// <summary>
        /// Gets a BRF association with its properties included
        /// </summary>
        /// <param name="id">The BRF association ID</param>
        /// <returns>The BRF association with properties if found, null otherwise</returns>
        Task<BrfAssociation> GetWithPropertiesAsync(Guid id);

        /// <summary>
        /// Gets a BRF association with its board members included
        /// </summary>
        /// <param name="id">The BRF association ID</param>
        /// <returns>The BRF association with board members if found, null otherwise</returns>
        Task<BrfAssociation> GetWithBoardMembersAsync(Guid id);

        /// <summary>
        /// Gets a BRF association with all related entities included
        /// </summary>
        /// <param name="id">The BRF association ID</param>
        /// <returns>The BRF association with all related entities if found, null otherwise</returns>
        Task<BrfAssociation> GetCompleteAsync(Guid id);

        /// <summary>
        /// Gets a BRF association by user ID (for board members)
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>The BRF association if the user is a board member, null otherwise</returns>
        Task<BrfAssociation> GetByUserIdAsync(string userId);

        /// <summary>
        /// Gets all BRF associations with their properties included
        /// </summary>
        /// <returns>A list of BRF associations with their properties</returns>
        Task<IReadOnlyList<BrfAssociation>> GetAllWithPropertiesAsync();
    }
}
