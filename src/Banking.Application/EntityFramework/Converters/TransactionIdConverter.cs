using Banking.Core.Transactions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Banking.Application.EntityFramework.Converters;

public sealed class TransactionIdConverter : ValueConverter<TransactionId, Guid>
{
    public TransactionIdConverter() : base(v => v.Value, v => TransactionId.From(v))
    {
    }
}