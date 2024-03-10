namespace Banking.Core;

public interface IAggregateRoot<out TId> : IEntity<TId> where TId : IEquatable<TId>
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
}