using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Core.Enums;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;

namespace SamtryggBrfPortal.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Property entities
    /// </summary>
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Property>> GetByBrfIdAsync(Guid brfId)
        {
            return await _dbSet
                .Where(p => p.BrfAssociationId == brfId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Property>> GetByOwnerIdAsync(string userId)
        {
            return await _dbSet
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Property> GetWithImagesAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc/>
        public async Task<Property> GetWithApplicationsAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.RentalApplications)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc/>
        public async Task<Property> GetCompleteAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Images)
                .Include(p => p.RentalApplications)
                .Include(p => p.BrfAssociation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Property>> GetAvailableByBrfIdAsync(Guid brfId)
        {
            return await _dbSet
                .Where(p => p.BrfAssociationId == brfId && p.IsAvailableForRent)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Property>> GetWithActiveApplicationsAsync(Guid brfId)
        {
            return await _dbSet
                .Include(p => p.RentalApplications)
                .Where(p => p.BrfAssociationId == brfId && 
                       p.RentalApplications.Any(a => a.Status == RentalStatus.Pending))
                .ToListAsync();
        }
    }
}
