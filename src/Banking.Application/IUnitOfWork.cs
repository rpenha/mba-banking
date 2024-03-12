namespace Banking.Application;

public interface IUnitOfWork : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}