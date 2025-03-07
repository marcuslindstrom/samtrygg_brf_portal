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
    /// Repository implementation for RentalApplication entities
    /// </summary>
    public class RentalApplicationRepository : GenericRepository<RentalApplication>, IRentalApplicationRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public RentalApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RentalApplication>> GetByPropertyIdAsync(Guid propertyId)
        {
            return await _dbSet
                .Where(a => a.PropertyId == propertyId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RentalApplication>> GetByBrfIdAsync(Guid brfId)
        {
            return await _dbSet
                .Include(a => a.Property)
                .Where(a => a.Property.BrfAssociationId == brfId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RentalApplication>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(a => a.ApplicantUserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RentalApplication>> GetByStatusAsync(RentalStatus status)
        {
            return await _dbSet
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<RentalApplication>> GetPendingByBrfIdAsync(Guid brfId)
        {
            return await _dbSet
                .Include(a => a.Property)
                .Where(a => a.Property.BrfAssociationId == brfId && a.Status == RentalStatus.Pending)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<RentalApplication> GetCompleteAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Property)
                    .ThenInclude(p => p.BrfAssociation)
                .Include(a => a.Documents)
                .Include(a => a.Messages)
                .Include(a => a.BackgroundCheck)
                    .ThenInclude(b => b.Documents)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <inheritdoc/>
        public async Task UpdateStatusAsync(Guid id, RentalStatus status, string updatedByUserId)
        {
            var application = await GetByIdAsync(id);
            if (application != null)
            {
                application.Status = status;
                application.LastStatusChangeByUserId = updatedByUserId;
                application.LastStatusChangeAt = System.DateTime.UtcNow;
                
                _context.Entry(application).State = EntityState.Modified;
            }
        }
    }
}
