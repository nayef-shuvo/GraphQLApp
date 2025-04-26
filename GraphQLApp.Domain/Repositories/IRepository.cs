using System.Linq.Expressions;
using GraphQLApp.Base;
using GraphQLApp.Base.Abstractions;

namespace GraphQLApp.Repositories;

public interface IRepository<T, in TId> where T : IBaseEntity<TId> where TId : struct
{
    Task<T?> GetByIdAsync(TId id, bool includeDeleted = false);

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = null,
        bool includeDeleted = false
    );

    Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        bool includeDeleted = false
    );

    Task InsertAsync(T entity);
    Task InsertManyAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateManyAsync(IEnumerable<T> entities);
    Task DeleteAsync(TId id);
    Task DeleteAsync(T entity);
    Task DeleteManyAsync(IEnumerable<T> entities);
    Task<IQueryable<T>> AsQueryableAsync();
}