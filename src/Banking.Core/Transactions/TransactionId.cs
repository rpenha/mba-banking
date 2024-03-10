namespace Banking.Core.Transactions;

public readonly record struct TransactionId
{
    private TransactionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static TransactionId Empty => new(Guid.Empty);

    public static TransactionId NewId() => new(Guid.NewGuid());

    public static TransactionId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid ID: {value}", nameof(value));

        return new(value);
    }

    public static implicit operator TransactionId(Guid value) => From(value);

    public static implicit operator Guid(TransactionId id) => id.Value;
}