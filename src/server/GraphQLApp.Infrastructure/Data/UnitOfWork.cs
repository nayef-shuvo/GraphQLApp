using GraphQLApp.Persistence;

namespace GraphQLApp.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        return new EfCoreTransaction(_dbContext, transaction);
    }
}