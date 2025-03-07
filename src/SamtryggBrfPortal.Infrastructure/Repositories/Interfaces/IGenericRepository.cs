using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SamtryggBrfPortal.Core.Entities;

namespace SamtryggBrfPortal.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>A list of all entities</returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets entities based on a filter expression
        /// </summary>
        /// <param name="filter">The filter expression</param>
        /// <returns>A list of entities that match the filter</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Checks if any entity matches the specified filter
        /// </summary>
        /// <param name="filter">The filter expression</param>
        /// <returns>True if any entity matches the filter, false otherwise</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Counts entities that match the specified filter
        /// </summary>
        /// <param name="filter">The filter expression (optional)</param>
        /// <returns>The count of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
