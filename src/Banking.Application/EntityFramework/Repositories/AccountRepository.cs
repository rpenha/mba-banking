using Banking.Core.Accounts;

namespace Banking.Application.EntityFramework.Repositories;

public class AccountRepository : EntityFrameworkRespository<AccountId, Account, BankingDbContext>, IAccountRepository
{
    public AccountRepository(BankingDbContext dbContext) : base(dbContext)
    {
    }
}