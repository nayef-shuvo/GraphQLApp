using GraphQLApp.Base;

namespace GraphQLApp.Persistence;

public interface IUnitOfWork : IScopedDependency
{
    Task<ITransaction> BeginTransactionAsync();
}