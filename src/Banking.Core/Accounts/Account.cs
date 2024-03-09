namespace Banking.Core.Accounts;

public class Account : AggregateRoot<Guid>
{
    public int CustomerId { get; private init; }
    public string BankBranch { get; private init; }
    public string AccountNumber { get; private init; }
    public decimal TotalLimit { get; private init; }
}