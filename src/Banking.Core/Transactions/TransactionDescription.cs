namespace Banking.Core.Transactions;

public readonly record struct TransactionDescription
{
    public const int MaxLength = 64;
    private readonly NormalizedString _value;

    public TransactionDescription(string value)
        : this(new NormalizedString(value))
    {
    }

    public TransactionDescription(NormalizedString value)
    {
        _value = value;
    }

    public static TransactionDescription From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Invalid transaction description: {value}", nameof(value));
        
        var input = value.ToUpperInvariant()
                         .RemoveDiacritics()
                         .Truncate(MaxLength);
        
        return new TransactionDescription(input!);
    }

    public override string ToString() => _value;

    public static implicit operator string(TransactionDescription value) => value.ToString();

    public static implicit operator TransactionDescription(string value) => From(value);
}