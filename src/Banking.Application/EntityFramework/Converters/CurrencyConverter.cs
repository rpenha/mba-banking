using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaMoney;

namespace Banking.Application.EntityFramework.Converters;

public sealed class CurrencyConverter : ValueConverter<Currency, string>
{
    public CurrencyConverter() : base(v => v.Code, v => Currency.FromCode(v))
    {
    }
}