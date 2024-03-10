using NodaMoney;

namespace Banking.Core.Accounts;

public abstract class BankAccount : AggregateRoot<BankAccountId>
{
    protected BankAccount()
    {
    }

    protected BankAccount(BankAccountId id, BankBranch bankBranch, BankAccountNumber accountNumber, Currency currency)
        : base(id)
    {
        Currency = currency;
        BankBranch = bankBranch;
        AccountNumber = accountNumber;
        Balance = new Money(0m, Currency);
    }

    public Currency Currency { get; private set; } = Currency.FromCode("BRL");
    
    public BankBranch BankBranch { get; private init; }

    public BankAccountNumber AccountNumber { get; private init; }

    public Money Balance { get; protected set; }
}