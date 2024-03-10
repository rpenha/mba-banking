using Banking.Application.EntityFramework.Converters;
using Banking.Application.EntityFramework.Mappings;
using Banking.Core.Accounts;
using Banking.Core.Customers;
using Microsoft.EntityFrameworkCore;
using NodaMoney;

namespace Banking.Application.EntityFramework;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerMap());
        modelBuilder.ApplyConfiguration(new BankAccountMap());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // var assembly = GetType().Assembly;
        // configurationBuilder.RegisterValueConverters(assembly);

        configurationBuilder.ComplexProperties<Money>();
        
        configurationBuilder.Properties<Currency>()
                            .HaveConversion<CurrencyConverter>()
                            .HaveMaxLength(8);
        
        configurationBuilder.Properties<CustomerId>().HaveConversion<CustomerIdConverter>();
        configurationBuilder.Properties<BankAccountId>().HaveConversion<BankAccountIdConverter>();
    }
}