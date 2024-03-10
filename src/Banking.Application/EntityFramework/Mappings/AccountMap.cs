using Banking.Core.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Application.EntityFramework.Mappings;

public sealed class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasDiscriminator<string>("account_type")
               .HasValue<CheckingAccount>("checking_account");

        builder.Property(x => x.Version)
               .IsRowVersion();

        builder.Property(x => x.BankBranch)
               .HasConversion(branch => branch.ToString(),
                              value => BankBranch.From(value));

        builder.Property(x => x.BankBranch)
               .HasConversion(branch => branch.ToString(),
                              value => BankBranch.From(value))
               .HasMaxLength(4)
               .IsRequired();

        builder.Property(x => x.AccountNumber)
               .HasConversion(numb => numb.ToString(),
                              value => AccountNumber.From(value))
               .HasMaxLength(7)
               .IsRequired();

        builder.HasIndex(x => new { x.BankBranch, x.AccountNumber });
    }
}