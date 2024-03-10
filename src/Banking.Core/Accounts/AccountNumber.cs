namespace Banking.Core.Accounts;

public readonly record struct AccountNumber
{
    private readonly string _value;
    private const int NUMBER_LENGTH = 6;
    private const int TOTAL_LENGTH = 7;
    private static readonly AccountNumber Empty = new(new string('0', TOTAL_LENGTH + 1));

    private AccountNumber(string value)
    {
        _value = value;
    }

    // public string AccountNumber => _value[..NUMBER_LENGTH];
    //
    // public char VerificationDigit => _value.Last();

    public static AccountNumber From(string input)
    {
        if (!IsValidAccountNumber(input))
            throw new ArgumentException($"Invalid bank account number: {input}", nameof(input));

        return new AccountNumber(input);
    }

    public static AccountNumber NewAccountNumber()
    {
        // Generate random account number string with desired length
        var number = string.Join("", Enumerable.Repeat("0123456789", NUMBER_LENGTH).Select(s => s[new Random().Next(s.Length)]));
        var digit = ComputeVerificationDigit(number);
        return new AccountNumber($"{number}{digit}");
    }

    private static bool IsValidAccountNumber(string input)
    {
        if (input.Length != TOTAL_LENGTH) return false;
        var number = input[..NUMBER_LENGTH];
        var dac = input.Last();
        return dac == ComputeVerificationDigit(number);
    }

    private static char ComputeVerificationDigit(string accountNumber)
    {
        // Implement modulus 11 calculation for verification digit
        // Refer to banking industry standards for specific algorithm details
        // Here's a placeholder for a simple modulus 11 calculation example:
        var sum = accountNumber.Sum(c => (int)char.GetNumericValue(c));
        var remainder = sum % 11;
        return (char)(remainder + 48); // Convert remainder to numerical digit character
    }

    public static bool TryParse(string input, out AccountNumber parsedAccountNumber)
    {
        try
        {
            // Attempt to create a BankAccountNumber instance
            AccountNumber accountNumber = input;
            parsedAccountNumber = accountNumber;
            return true;
        }
        catch (ArgumentException)
        {
            parsedAccountNumber = Empty; // Provide a default value
            return false;
        }
    }

    public override string ToString() => _value;

    public static implicit operator AccountNumber(string accountNumber) => From(accountNumber);

    public static implicit operator string(AccountNumber accountNumber) => accountNumber.ToString();
}