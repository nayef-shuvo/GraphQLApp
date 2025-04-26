using System.Linq.Expressions;
using GraphQLApp.Base.Abstractions;
using GraphQLApp.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApp.Repositories;

public class EfCoreRepository<T, TId> : IRepository<T, TId> where T : class, IBaseEntity<TId> where TId : struct
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public EfCoreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate is null)
            return await _dbSet.FirstOrDefaultAsync();

        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate is null)
            return await _dbSet.ToListAsync();

        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task InsertManyAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateManyAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
            return;

        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteManyAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await SaveChangesAsync();
    }

    public async Task<IQueryable<T>> AsQueryableAsync()
    {
        return await Task.FromResult(_dbSet.AsQueryable());
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}