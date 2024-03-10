using Banking.Core.Customers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Banking.Application.EntityFramework.Converters;

public sealed class CustomerIdConverter : ValueConverter<CustomerId, Guid>
{
    public CustomerIdConverter() : base(v => v.Value, v => CustomerId.From(v))
    {
    }
}