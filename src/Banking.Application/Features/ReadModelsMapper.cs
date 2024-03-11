using Banking.Core.Customers;
using Banking.Core.Transactions;
using Riok.Mapperly.Abstractions;

namespace Banking.Application.Features;

[Mapper]
public static partial class ReadModelsMapper
{
    [MapperIgnoreSource(nameof(Customer.Name))]
    public static partial ReadModels.Customer ToReadModel(this Customer customer);
    
    [MapProperty($"{nameof(Transaction.Value)}.{nameof(Transaction.Value.Amount)}", nameof(ReadModels.Transaction.Amount))]
    [MapProperty($"{nameof(Transaction.Value)}.{nameof(Transaction.Value.Currency)}.{nameof(Transaction.Value.Currency.Code)}", nameof(ReadModels.Transaction.Currency))]
    public static partial ReadModels.Transaction ToReadModel(this Transaction transaction);
}