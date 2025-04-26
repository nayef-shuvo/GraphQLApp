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

    public async Task<T?> GetByIdAsync(TId id, bool includeDeleted = false)
    {
        var query = await AsQueryableAsync();

        if (!includeDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            query = query.Where(e => !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted)));

        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
        bool includeDeleted = false)
    {
        var query = await AsQueryableAsync();

        if (!includeDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            query = query.Where(e => !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted)));

        if (predicate is null)
            return await query.FirstOrDefaultAsync();

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        bool includeDeleted = false)
    {
        var query = await AsQueryableAsync();

        if (!includeDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            query = query.Where(e => !EF.Property<bool>(e, nameof(ISoftDelete.IsDeleted)));

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
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

        await DeleteAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
            await UpdateAsync(entity);
        }
        else
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    public async Task DeleteManyAsync(IEnumerable<T> entities)
    {
        var baseEntities = entities.ToArray();

        if (baseEntities.Length == 0)
            return;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
        {
            foreach (var entity in baseEntities)
            {
                if (entity is ISoftDelete softDeletable)
                {
                    softDeletable.IsDeleted = true;
                }
            }

            await UpdateManyAsync(baseEntities);
        }
        else
        {
            _dbSet.RemoveRange(baseEntities);
            await SaveChangesAsync();
        }
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