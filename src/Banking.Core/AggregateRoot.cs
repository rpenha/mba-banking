using LanguageExt;

namespace Banking.Core;

public interface IAggregateRoot<out TId> : IEntity<TId> where TId : IEquatable<TId>
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
}

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : IEquatable<TId>
{
    // ReSharper disable once UnusedMember.Global
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }

    public DateTimeOffset CreatedAt { get; private init; } = DateTimeProvider.Now;
    public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeProvider.Now;
}

public interface IRepository<in TId, TAggregateRoot>
    where TId : IEquatable<TId>
    where TAggregateRoot : IAggregateRoot<TId>
{
    Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);

    Task<Option<TAggregateRoot>> LoadAsync(TId id, CancellationToken cancellationToken = default);

    IUnitOfWork GetUnitOfWork();
}

public interface IUnitOfWork : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}