using Microsoft.EntityFrameworkCore.Storage;

namespace Waffle.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        /// <summary>
        /// Finds all entities of <typeparamref name="T" /> from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<T?> FindAsync(object id);
        Task<IReadOnlyList<T>> ListAsync();
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(IReadOnlyList<T> entities);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync();
        Task<bool> AnyAsync();
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        IQueryable<T> Queryable();
    }
}
