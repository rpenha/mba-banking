namespace Banking.Core.Customers;

public readonly record struct CustomerId
{
    private CustomerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CustomerId Empty => new(Guid.Empty);

    public static CustomerId NewId() => new(Guid.NewGuid());

    public static CustomerId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid ID: {value}", nameof(value));

        return new(value);
    }

    public static implicit operator CustomerId(Guid value) => From(value);

    public static implicit operator Guid(CustomerId id) => id.Value;
}