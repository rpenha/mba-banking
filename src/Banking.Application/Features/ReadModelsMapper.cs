using Banking.Application.Features.Customers;
using Banking.Core.Customers;
using Riok.Mapperly.Abstractions;

namespace Banking.Application.Features;

[Mapper]
public static partial class ReadModelsMapper
{
    [MapperIgnoreSource(nameof(Customer.Name))]
    public static partial ReadModels.Customer ToReadModel(this Customer customer);
}