namespace Banking.Core.Transactions;

public enum DepositType
{
    // ATM deposit - This might be followed by the location of the ATM if available.
    Atm,
    // Mobile deposit - This indicates you deposited a check using your bank's mobile
    MobileDeposit,
    // Direct deposit - This refers to a recurring electronic deposit, often for salary or benefits. You might see the name of the payer listed.
    DirectDeposit,
    // Cash deposit - This specifies that you deposited cash through a teller at a bank branch.
    CashDeposit,
    // Check deposit - This indicates you deposited a physical check at a branch or ATM.
    CheckDeposit,
    // ACH transfer - This describes an electronic transfer where the funds are moving into the account. You might see the name of the sender.
    AchTransfer,
    // Transfer from another account - This indicates a transfer of funds from another one of your accounts at the same bank.
    TransferFromAnotherAccount
}