using Banking.Core.Customers;

namespace Banking.Application.EntityFramework.Repositories;

public class CustomerRepository : EntityFrameworkRespository<CustomerId, Customer, BankingDbContext>, ICustomerRepository
{
    public CustomerRepository(BankingDbContext dbContext) : base(dbContext)
    {
    }
}