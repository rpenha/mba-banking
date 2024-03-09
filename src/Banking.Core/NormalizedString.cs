namespace Banking.Core;

public readonly record struct NormalizedString
{
    private readonly string _value;

    public NormalizedString(string value)
    {
        var input = value.Trim().Normalize();
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException(nameof(value));
        _value = input;
    }

    public override string ToString() => _value;

    public static implicit operator string(NormalizedString value) => value.ToString();
    
    public static implicit operator NormalizedString(string value) => new(value);
}