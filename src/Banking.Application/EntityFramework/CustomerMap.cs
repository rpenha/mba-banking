using Banking.Core;
using Banking.Core.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Banking.Application.EntityFramework;

public sealed class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasIndex(c => c.TaxId)
               .IsUnique();

        builder.Property(c => c.TaxId)
               .IsRequired()
               .HasMaxLength(14)
               .HasConversion(taxId => taxId.ToString(),
                              value => TaxId.From(value));

        builder.ComplexProperty(c => c.Name,
                                cb =>
                                {
                                    cb.Property(x => x.FirstName)
                                      .HasMaxLength(20)
                                      .IsRequired();

                                    cb.Property(x => x.LastName)
                                      .HasMaxLength(50)
                                      .IsRequired();
                                });

        builder.Property(c => c.DateOfBirth)
               .HasConversion(dob => dob.Date,
                              date => new DateOfBirth(date))
               .IsRequired();
    }
}