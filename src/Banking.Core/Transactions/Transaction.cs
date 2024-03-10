using Banking.Core.Accounts;
using NodaMoney;

namespace Banking.Core.Transactions;

public abstract class Transaction : AggregateRoot<TransactionId>
{
    protected Transaction()
    {
    }
    protected Transaction(TransactionId id, AccountId accountId, Money value, TransactionDescription description) 
        : base(id)
    {
        AccountId = accountId;
        Value = value;
        Description = description;
        Timestamp = DateTimeProvider.Now;
    }

    public AccountId AccountId { get; private init; }
    public Money Value { get; private init; }
    public DateTimeOffset Timestamp { get; private init; }
    public TransactionDescription Description { get; private init; }
    
    public abstract TransactionDirection Direction { get; protected init; }
}