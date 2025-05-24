using GraphQLApp.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApp.Data;

public class EfCoreTransaction : ITransaction
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDbContextTransaction _transaction;

    public EfCoreTransaction(ApplicationDbContext dbContext, IDbContextTransaction transaction)
    {
        _dbContext = dbContext;
        _transaction = transaction;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _transaction.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}