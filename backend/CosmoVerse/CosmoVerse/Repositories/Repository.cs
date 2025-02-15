using CosmoVerse.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CosmoVerse.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly UserDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(UserDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id">Id of user or key</param>
        /// <returns>The data corresponding with the Id</returns>
        public async Task<T?> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }


        /// <summary>
        /// Find entity by expression
        /// </summary>
        /// <param name="expression">Expression to filter the data</param>
        /// <returns>The data corresponding with the expression</returns>
        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }


        /// <summary>
        /// Find all entities by expression
        /// </summary>
        /// <param name="expression">Expression to filter the data</param>
        /// <returns>Every data corresponding with the expression</returns>
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }


        /// <summary>
        /// Get all entities
        /// </summary> 
        /// <returns>Every data in the database</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        /// <summary>
        /// Add entity to the database
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
        }


        /// <summary>
        /// Update entity in the database
        /// </summary>
        /// <param name="entity">Entity to update</param>
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }


        /// <summary>
        /// Delete entity from the database
        /// </summary>
        /// <param name="id">Id of entity to delete</param>
        public async Task DeleteAsync(Guid id)
        {
            // Find entity by id
            var entity = await FindByIdAsync(id);

            // Remove entity if found
            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
        }


        /// <summary>
        /// Save changes to the database
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
