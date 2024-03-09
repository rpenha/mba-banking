using Banking.Core;
using Banking.Core.Customers;
using Microsoft.EntityFrameworkCore;

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
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        var assembly = GetType().Assembly;
        //configurationBuilder.RegisterValueConverters(assembly);
    }
}