using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Core.Interfaces;

namespace Waffle.Core.Foundations;

/// <summary>
/// "There's some repetition here - couldn't we have some the sync methods call the async?"
/// https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/
/// </summary>
/// <typeparam name="T"></typeparam>
public class EfRepository<T> : IAsyncRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public EfRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T?> FindAsync(object id) => await _context.Set<T>().FindAsync(id);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<T>> ListAsync() => await _context.Set<T>().ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

    public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<int> AddRangeAsync(IReadOnlyList<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync() => await _context.Set<T>().AnyAsync();

    public IQueryable<T> Queryable() => _context.Set<T>();
}
