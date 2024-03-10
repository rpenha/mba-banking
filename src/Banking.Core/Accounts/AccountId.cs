namespace Banking.Core.Accounts;

public readonly record struct AccountId
{
    private AccountId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static AccountId Empty => new(Guid.Empty);

    public static AccountId NewId() => new(Guid.NewGuid());

    public static AccountId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid ID: {value}", nameof(value));

        return new(value);
    }

    public static implicit operator AccountId(Guid value) => From(value);

    public static implicit operator Guid(AccountId id) => id.Value;
}