using Banking.Core.Accounts;
using Humanizer;
using NodaMoney;

namespace Banking.Core.Transactions;

public class Deposit : Transaction
{
    protected Deposit()
    {
    }

    public Deposit(TransactionId id, AccountId accountId, Money value, TransactionDescription description)
        : base(id, accountId, value, description)
    {
        Direction = TransactionDirection.CashIn;
    }

    public sealed override TransactionDirection Direction { get; protected init; }

    public static Deposit NewDeposit(AccountId accountId, Money value, DepositType depositType)
    {
        var id = TransactionId.NewId();
        var description = depositType.ToString().Humanize(LetterCasing.AllCaps);
        return new Deposit(id, accountId, value, description);
    }
}