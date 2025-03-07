using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;

namespace SamtryggBrfPortal.Infrastructure.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing transactions and repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="brfAssociationRepository">BRF Association repository</param>
        /// <param name="propertyRepository">Property repository</param>
        /// <param name="rentalApplicationRepository">Rental Application repository</param>
        public UnitOfWork(
            ApplicationDbContext context,
            IBrfAssociationRepository brfAssociationRepository,
            IPropertyRepository propertyRepository,
            IRentalApplicationRepository rentalApplicationRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            BrfAssociations = brfAssociationRepository ?? throw new ArgumentNullException(nameof(brfAssociationRepository));
            Properties = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
            RentalApplications = rentalApplicationRepository ?? throw new ArgumentNullException(nameof(rentalApplicationRepository));
        }

        /// <inheritdoc/>
        public IBrfAssociationRepository BrfAssociations { get; }

        /// <inheritdoc/>
        public IPropertyRepository Properties { get; }

        /// <inheritdoc/>
        public IRentalApplicationRepository RentalApplications { get; }

        /// <inheritdoc/>
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
