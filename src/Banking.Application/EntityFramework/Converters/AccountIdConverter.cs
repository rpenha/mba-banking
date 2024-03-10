using Banking.Core.Accounts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Banking.Application.EntityFramework.Converters;

public sealed class AccountIdConverter : ValueConverter<AccountId, Guid>
{
    public AccountIdConverter() : base(v => v.Value, v => AccountId.From(v))
    {
    }
}