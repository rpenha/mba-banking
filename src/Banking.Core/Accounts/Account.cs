using Banking.Core.Customers;
using NodaMoney;

namespace Banking.Core.Accounts;

public abstract class Account : AggregateRoot<AccountId>
{
    protected Account()
    {
    }

    protected Account(AccountId id, CustomerId customerId, BankBranch bankBranch, AccountNumber accountNumber, Currency currency)
        : base(id)
    {
        CustomerId = customerId;
        Currency = currency;
        BankBranch = bankBranch;
        AccountNumber = accountNumber;
        Balance = new Money(0m, Currency);
    }

    public CustomerId CustomerId { get; private init; }

    public Currency Currency { get; private set; } = Currency.FromCode("BRL");

    public BankBranch BankBranch { get; private init; }

    public AccountNumber AccountNumber { get; private init; }

    public Money Balance { get; private set; }

    public virtual void Deposit(Money value)
    {
        CheckValidTransactionValue(value);
        Balance += value;
        UpdatedAt = DateTimeProvider.Now;
    }
    
    public virtual bool Withdraw(Money value)
    {
        CheckValidTransactionValue(value);
        if (value > Balance) return false;
        Balance -= value;
        UpdatedAt = DateTimeProvider.Now;
        return true;
    }

    protected void CheckValidTransactionValue(Money value)
    {
        if (value < new Money(0m, Currency) || value.Currency != Currency)
            throw new ArgumentException($"Invalid value for transaction: {value}", nameof(value));
    }
}

public interface IDepositTransaction
{
    Money Value { get; }
}

public interface IWithdrawTransaction
{
    Money Value { get; }
}