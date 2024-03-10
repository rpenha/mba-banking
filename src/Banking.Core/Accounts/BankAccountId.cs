namespace Banking.Core.Accounts;

public readonly record struct BankAccountId
{
    private BankAccountId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BankAccountId Empty => new(Guid.Empty);

    public static BankAccountId NewId() => new(Guid.NewGuid());

    public static BankAccountId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"Invalid ID: {value}", nameof(value));

        return new(value);
    }

    public static implicit operator BankAccountId(Guid value) => From(value);

    public static implicit operator Guid(BankAccountId id) => id.Value;
}