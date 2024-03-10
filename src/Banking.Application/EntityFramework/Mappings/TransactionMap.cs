using Banking.Core.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Application.EntityFramework.Mappings;

public sealed class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasDiscriminator<string>("transaction_type")
               .HasValue<Deposit>("deposit")
               .HasValue<Withdraw>("withdraw");

        builder.Property(x => x.Version)
               .IsRowVersion();

        builder.Property(x => x.Description)
               .HasMaxLength(TransactionDescription.MaxLength)
               .HasConversion(desc => desc.ToString(),
                              value => TransactionDescription.From(value));

        builder.HasIndex(x => new { x.AccountId, x.Timestamp })
               .IsDescending([false, true]);
    }
}