namespace Banking.Core.Transactions;

public enum WithdrawType
{
    // ATM withdrawal - This could be followed by the location of the ATM, if available.
    Atm,
    // Debit card purchase - This will typically include the name of the merchant where the purchase was made.
    DebitCardPurchase,
    // ACH transfer - This stands for Automated Clearing House and refers to electronic transfers between bank accounts. You might see the name of the recipient along with the transfer.
    AchTransfer,
    // Wire transfer - This is another type of electronic transfer, often used for larger sums of money. The description might mention the reason for the transfer, such as "rent payment."
    WireTransfer,
    // Check withdrawal - This indicates that a check was cashed and the funds were withdrawn from your account.
    CheckWithdrawal,
    // Cash withdrawal - Bank branch - This specifies that you withdrew cash from a teller at a bank branch.
    CashWithdrawal
}