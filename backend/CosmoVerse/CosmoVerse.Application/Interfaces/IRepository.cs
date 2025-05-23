﻿using System.Linq.Expressions;

namespace CosmoVerse.Application.Interfaces
{
    public interface IRepository<T, TId> where T : class
    {
        Task<T?> FindByIdAsync(TId id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<TResult>> FindWithProjectionAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TId id);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
