using System.Linq.Expressions;

namespace CosmoVerse.Repositories
{
    public interface IRepository<T, TId> where T : class
    {
        Task<T?> FindByIdAsync(TId id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TId id);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
