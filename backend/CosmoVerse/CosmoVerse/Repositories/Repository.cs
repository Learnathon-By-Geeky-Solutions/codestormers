﻿using CosmoVerse.Data;
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
        /// <returns>The entity if found, otherwise null.</returns>
        /// <exception cref="ArgumentException">Thrown if the ID is invalid.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during the database operation.</exception>
        public async Task<T?> FindByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be an empty GUID.", nameof(id));

            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finding entity of type {typeof(T).Name} with id {id}.", ex);
            }
        }


        /// <summary>
        /// Finds an entity based on a given filter expression.
        /// </summary>
        /// <param name="expression">A LINQ expression to filter the data.</param>
        /// <returns>The first entity matching the expression, or null if no match is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the expression is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during the database operation.</exception>
        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "The filter expression cannot be null.");

            try
            {
                return await _dbSet.FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finding entity of type {typeof(T).Name} with the given expression.", ex);
            }
        }

        /// <summary>
        /// Finds all entities matching a given filter expression.
        /// </summary>
        /// <param name="expression">A LINQ expression to filter the data.</param>
        /// <returns>A list of entities that match the filter expression. Returns an empty list if no matches are found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the expression is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during the database operation.</exception>
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "The filter expression cannot be null.");

            try
            {
                return await _dbSet.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finding all entities of type {typeof(T).Name} with the given expression.", ex);
            }
        }

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>A list containing all entities. Returns an empty list if no data exists.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the database operation.</exception>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all entities of type {typeof(T).Name}.", ex);
            }
        }


        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while adding the entity.</exception>
        public async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The entity to add cannot be null.");

            try
            {
                await _dbSet.AddAsync(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding entity of type {typeof(T).Name}.", ex);
            }
        }


        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
        /// <exception cref="DbUpdateException">Thrown if an error occurs during the database update.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs.</exception>
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "The entity to update cannot be null.");

            try
            {
                _dbSet.Update(entity);
                await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                
                throw new Exception($"Database update failed for entity of type {typeof(T).Name}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating entity of type {typeof(T).Name}.", ex);
            }
        }



        /// <summary>
        /// Delete entity from the database
        /// </summary>
        /// <param name="id">Id of entity to delete</param>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                // Find entity by id
                var entity = await FindByIdAsync(id);

                // Throw exception if entity not found
                if (entity == null)
                {
                    throw new ArgumentException($"Entity with id {id} not found.", nameof(id)); // Custom exception for not found
                }

               
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Error: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Database update failed while deleting entity with id {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity", ex);
            }
        }



        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
        /// <exception cref="DbUpdateException">Thrown if an error occurs during the database update.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs.</exception>
        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The entity to delete cannot be null.");
            }


            try
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Database update failed while deleting entity of type {typeof(T).Name}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity of type {typeof(T).Name}.", ex);
            }
        }




        /// <summary>
        /// Save changes to the database.
        /// </summary>
        /// <exception cref="DbUpdateException">Thrown if an error occurs during the save operation.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs.</exception>
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while saving changes.", ex);
            }
        }
    }
}
