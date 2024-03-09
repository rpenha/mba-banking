namespace Banking.Core.Customers;

public readonly record struct DateOfBirth
{
    public DateOfBirth(DateOnly date)
    {
        var minDate = DateOnly.FromDateTime(DateTimeProvider.Now.AddYears(-100).DateTime);
        var maxDate = DateOnly.FromDateTime(DateTimeProvider.Now.AddYears(-18).DateTime);
        if (date < minDate || date > maxDate)
            throw new ArgumentOutOfRangeException(nameof(date), date, $"Date of birth must be between {minDate} and {maxDate}");
        Date = date;
    }

    public DateOnly Date { get; }

    public static implicit operator DateOnly(DateOfBirth value) => value.Date;

    public static implicit operator DateOfBirth(DateOnly value) => new(value);
    
    public static implicit operator DateOfBirth(DateTime value) => new(DateOnly.FromDateTime(value));
    
    public static implicit operator DateOfBirth(DateTimeOffset value) => new(DateOnly.FromDateTime(value.DateTime));
}