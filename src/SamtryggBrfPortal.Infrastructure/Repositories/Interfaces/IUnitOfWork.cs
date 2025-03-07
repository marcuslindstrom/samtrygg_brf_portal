using System;
using System.Threading.Tasks;

namespace SamtryggBrfPortal.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work interface for managing transactions and repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// BRF Association repository
        /// </summary>
        IBrfAssociationRepository BrfAssociations { get; }

        /// <summary>
        /// Property repository
        /// </summary>
        IPropertyRepository Properties { get; }

        /// <summary>
        /// Rental Application repository
        /// </summary>
        IRentalApplicationRepository RentalApplications { get; }

        /// <summary>
        /// Saves all changes made in this unit of work to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> CompleteAsync();

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the transaction
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task RollbackTransactionAsync();
    }
}
