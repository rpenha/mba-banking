using Banking.Core.Accounts;

namespace Banking.Application.EntityFramework.Repositories;

public class BankAccountRepository : EntityFrameworkRespository<BankAccountId, BankAccount, BankingDbContext>, IBankAccountRepository
{
    public BankAccountRepository(BankingDbContext dbContext) : base(dbContext)
    {
    }
}