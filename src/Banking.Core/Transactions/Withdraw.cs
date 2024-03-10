using Banking.Core.Accounts;
using Humanizer;
using NodaMoney;

namespace Banking.Core.Transactions;

public class Withdraw : Transaction
{
    protected Withdraw()
    {
    }

    public Withdraw(TransactionId id, AccountId accountId, Money value, TransactionDescription description)
        : base(id, accountId, value, description)
    {
        Direction = TransactionDirection.CashOut;
    }

    public sealed override TransactionDirection Direction { get; protected init; }

    public static Withdraw NewWithdraw(AccountId accountId, Money value, WithdrawType withdrawType)
    {
        var id = TransactionId.NewId();
        var description = withdrawType.ToString().Humanize(LetterCasing.AllCaps);
        return new Withdraw(id, accountId, value, description);
    }
}