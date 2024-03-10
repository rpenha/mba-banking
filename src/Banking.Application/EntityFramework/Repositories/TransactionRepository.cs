using Banking.Core.Transactions;

namespace Banking.Application.EntityFramework.Repositories;

public class TransactionRepository : EntityFrameworkRespository<TransactionId, Transaction, BankingDbContext>, ITransactionRepository
{
    public TransactionRepository(BankingDbContext dbContext) : base(dbContext)
    {
    }
}