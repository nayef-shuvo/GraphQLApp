namespace GraphQLApp.Persistence;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}