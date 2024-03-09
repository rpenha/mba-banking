namespace Banking.Core;

public interface IEntity<out TId> where TId : IEquatable<TId> 
{
    public TId Id { get;  }
}

public abstract class Entity<TId> : IEntity<TId> where TId : IEquatable<TId>
{
    protected Entity()
    {
    }
    
    protected Entity(TId id)
    {
        if (id.Equals(default))
            throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
        Id = id;
    }

    public TId Id { get; private init; } = default!;
}