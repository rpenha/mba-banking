using System.Text.RegularExpressions;

namespace Banking.Core.Accounts;

public readonly record struct BankBranch
{
    private readonly string _value;

    public BankBranch(string value)
    {
        _value = value;
    }

    public static BankBranch From(string input)
    {
        if (!Regex.IsMatch(input.AsSpan(), @"\d{4}"))
            throw new ArgumentException($"Invalid bank branch: {input}", nameof(input));

        return new BankBranch(input);
    }

    public override string ToString() => _value;

    public static implicit operator string(BankBranch value) => value.ToString();

    public static implicit operator BankBranch(string value) => From(value);
}