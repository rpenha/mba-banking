using Banking.Core.Accounts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Banking.Application.EntityFramework.Converters;

public sealed class BankAccountIdConverter : ValueConverter<BankAccountId, Guid>
{
    public BankAccountIdConverter() : base(v => v.Value, v => BankAccountId.From(v))
    {
    }
}