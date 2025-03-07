using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;

namespace SamtryggBrfPortal.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for BrfAssociation entities
    /// </summary>
    public class BrfAssociationRepository : GenericRepository<BrfAssociation>, IBrfAssociationRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public BrfAssociationRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<BrfAssociation> GetByOrgNumberAsync(string orgNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.OrganizationNumber == orgNumber);
        }

        /// <inheritdoc/>
        public async Task<BrfAssociation> GetWithPropertiesAsync(Guid id)
        {
            return await _dbSet
                .Include(b => b.Properties)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <inheritdoc/>
        public async Task<BrfAssociation> GetWithBoardMembersAsync(Guid id)
        {
            return await _dbSet
                .Include(b => b.BoardMembers)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <inheritdoc/>
        public async Task<BrfAssociation> GetCompleteAsync(Guid id)
        {
            return await _dbSet
                .Include(b => b.Properties)
                    .ThenInclude(p => p.Images)
                .Include(b => b.Properties)
                    .ThenInclude(p => p.RentalApplications)
                .Include(b => b.BoardMembers)
                .Include(b => b.Documents)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <inheritdoc/>
        public async Task<BrfAssociation> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(b => b.BoardMembers)
                .FirstOrDefaultAsync(b => b.BoardMembers.Any(m => m.UserId == userId));
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<BrfAssociation>> GetAllWithPropertiesAsync()
        {
            return await _dbSet
                .Include(b => b.Properties)
                .ToListAsync();
        }
    }
}
