using Banking.Core.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Application.EntityFramework.Mappings;

public sealed class CheckingAccountMap : IEntityTypeConfiguration<CheckingAccount>
{
    public void Configure(EntityTypeBuilder<CheckingAccount> builder)
    {
        builder.Property(x => x.IsUsingLimit).ValueGeneratedOnAddOrUpdate();
        builder.ComplexProperty(x => x.UsedLimit);
    }
}