namespace Banking.Core.Transactions;

public class Transaction : AggregateRoot<Guid>
{
    public int AccountId { get; private init; }
    public DateTimeOffset Timestamp { get; private init; }
    public decimal Amount { get; private init; }
    public string Description { get; private init; }
    public TransactionType TransactionType { get; private init; }
}