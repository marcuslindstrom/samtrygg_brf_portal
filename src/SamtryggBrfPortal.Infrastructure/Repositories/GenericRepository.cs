using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Infrastructure.Data;
using SamtryggBrfPortal.Infrastructure.Repositories.Interfaces;

namespace SamtryggBrfPortal.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type that inherits from BaseEntity</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        /// <inheritdoc/>
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.ModifiedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await _dbSet.CountAsync();
            }
            
            return await _dbSet.CountAsync(filter);
        }
    }
}
