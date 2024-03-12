using Banking.Core.Accounts;
using Banking.Core.Customers;
using Banking.Core.Transactions;
using Riok.Mapperly.Abstractions;

namespace Banking.Application.Features;

[Mapper]
public static partial class ReadModelsMapper
{
    [MapperIgnoreSource(nameof(Customer.Name))]
    public static partial ReadModels.Customer ToCustomerModel(this Customer source);
    
    [MapProperty($"{nameof(Transaction.Value)}.{nameof(Transaction.Value.Amount)}", nameof(ReadModels.Transaction.Amount))]
    [MapProperty($"{nameof(Transaction.Value)}.{nameof(Transaction.Value.Currency)}.{nameof(Transaction.Value.Currency.Code)}", nameof(ReadModels.Transaction.Currency))]
    public static partial ReadModels.Transaction ToTransactionModel(this Transaction source);
    
    
    [MapProperty($"{nameof(CheckingAccount.Currency)}.{nameof(CheckingAccount.Currency.Code)}", nameof(ReadModels.CheckingAccount.Currency))]
    [MapProperty($"{nameof(CheckingAccount.Balance)}.{nameof(CheckingAccount.Balance.Amount)}", nameof(ReadModels.CheckingAccount.Balance))]
    [MapProperty($"{nameof(CheckingAccount.TotalLimit)}.{nameof(CheckingAccount.TotalLimit.Amount)}", nameof(ReadModels.CheckingAccount.TotalLimit))]
    [MapProperty($"{nameof(CheckingAccount.CurrentLimit)}.{nameof(CheckingAccount.CurrentLimit.Amount)}", nameof(ReadModels.CheckingAccount.CurrentLimit))]
    [MapProperty($"{nameof(CheckingAccount.UsedLimit)}.{nameof(CheckingAccount.UsedLimit.Amount)}", nameof(ReadModels.CheckingAccount.UsedLimit))]
    public static partial ReadModels.CheckingAccount ToCheckingAccountModel(this CheckingAccount source);

    [MapperIgnoreSource(nameof(Account.Balance))]
    [MapperIgnoreSource(nameof(Account.CustomerId))]
    [MapProperty($"{nameof(Account.Currency)}.{nameof(Account.Currency.Code)}", nameof(ReadModels.CustomerAccount.Currency))]
    [MapProperty($"{nameof(Account.Currency)}.{nameof(Account.Currency.Code)}", nameof(ReadModels.CustomerAccount.Currency))]
    public static partial ReadModels.CustomerAccount ToCustomerAccountModel(this Account source);
}