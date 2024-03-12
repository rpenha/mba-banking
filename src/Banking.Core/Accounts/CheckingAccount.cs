using Banking.Core.Customers;
using NodaMoney;

namespace Banking.Core.Accounts;

public class CheckingAccount : Account
{
    protected CheckingAccount()
    {
    }

    private CheckingAccount(AccountId id,
                            CustomerId customerId,
                            BankBranch bankBranch,
                            AccountNumber accountNumber,
                            Money totalLimit) : base(id, customerId, bankBranch, accountNumber, totalLimit.Currency)
    {
        TotalLimit = totalLimit;
        CurrentLimit = TotalLimit;
    }

    public Money TotalLimit { get; private set; }

    public Money CurrentLimit { get; private set; }

    public bool IsUsingLimit
    {
        get => CurrentLimit < TotalLimit;
        
        // Write-only property
        // ReSharper disable once ValueParameterNotUsed
        private init { }
    }

    public Money UsedLimit
    {
        get { return TotalLimit - CurrentLimit; }
        
        // Write-only property
        // ReSharper disable once ValueParameterNotUsed
        private init { }
    }

    public static CheckingAccount NewCheckingAccount(CustomerId customerId, BankBranch bankBranch, Money totalLimit)
    {
        var id = AccountId.NewId();
        var accountNumber = AccountNumber.NewAccountNumber();
        return new CheckingAccount(id,
                                   customerId,
                                   bankBranch,
                                   accountNumber,
                                   totalLimit);
    }

    public override void Deposit(Money value)
    {
        CheckValidTransactionValue(value);

        var isUsingLimit = IsUsingLimit;
        var depositExceedsUsedLimit = value > UsedLimit;
        var constraints = (isUsingLimit, depositExceedsUsedLimit);

        switch (constraints)
        {
            case (false, _):
                base.Deposit(value);
                break;
            case (true, false):
                CurrentLimit += value;
                break;
            case (true, true):
                value -= UsedLimit;
                CurrentLimit = TotalLimit;
                base.Deposit(value);
                break;
        }
    }

    public override bool Withdraw(Money value)
    {
        if (value > Balance + CurrentLimit)
            return false;

        if (value <= Balance)
            return base.Withdraw(value);

        CurrentLimit -= value - Balance;
        return base.Withdraw(Balance);
    }
}