using Banking.Core.Customers;
using NodaMoney;

namespace Banking.Core.Accounts;

public class CheckingAccount : BankAccount
{
    protected CheckingAccount()
    {
    }

    private CheckingAccount(BankAccountId id,
                            CustomerId customerId,
                            BankBranch bankBranch,
                            BankAccountNumber accountNumber,
                            Money totalLimit) : base(id, bankBranch, accountNumber, totalLimit.Currency)
    {
        CustomerId = customerId;
        TotalLimit = totalLimit;
        CurrentLimit = TotalLimit;
    }

    public CustomerId CustomerId { get; private init; }
    public Money CurrentLimit { get; private set; }
    public Money TotalLimit { get; private set; }

    public static CheckingAccount NewCheckingAccount(CustomerId customerId, BankBranch bankBranch, Money totalLimit)
    {
        var id = BankAccountId.NewId();
        var accountNumber = BankAccountNumber.NewBankAccountNumber();
        return new CheckingAccount(id,
                                   customerId,
                                   bankBranch,
                                   accountNumber,
                                   totalLimit);
    }

    // public void SetTotalLimit(Money totalLimit)
    // {
    //     if (totalLimit < 0)
    //         throw new ArgumentException($"Invalid total limit value for account: {totalLimit}", nameof(totalLimit));
    //     if (totalLimit.Currency != Currency)
    //         throw new ArgumentException($"Invalid currency: {totalLimit.Currency}", nameof(totalLimit));
    //
    //     TotalLimit = totalLimit;
    //     UpdatedAt = DateTimeProvider.Now;
    // }
}