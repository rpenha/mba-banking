namespace Banking.Core;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : IEquatable<TId>
{
    // ReSharper disable once UnusedMember.Global
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
    
    public uint Version { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeProvider.Now;
    
    public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeProvider.Now;
}