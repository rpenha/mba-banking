namespace Banking.Core.Transactions;

public interface ITransactionRepository : IRepository<TransactionId, Transaction>;