using GraphQLApp.Persistence;

namespace GraphQLApp.Data.Extensions;

public static class UnitOfWorkExtensions
{
    public static async Task ExecuteInTransactionAsync(this IUnitOfWork uow, Func<Task> operation)
    {
        var transaction = await uow.BeginTransactionAsync();

        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
