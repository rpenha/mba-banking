using Banking.Application.EntityFramework;
using Banking.Core.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Riok.Mapperly.Abstractions;

namespace Banking.Application.Features.GetCustomer;

public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, GetCustomerResult>
{
    private readonly BankingDbContext _dbContext;

    public GetCustomerHandler(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<GetCustomerResult> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Customers
                                     .Where(x => x.Id == request.CustomerId)
                                     .Select(x => x.ToReadModel())
                                     .FirstOrDefaultAsync(cancellationToken);

        return result switch
               {
                   not null => new CustomerFound(result),
                   _ => new CustomerNotFound(request)
               };
    }
}

public readonly record struct GetCustomerQuery(Guid CustomerId) : IRequest<GetCustomerResult>;

[GenerateOneOf]
public partial class GetCustomerResult : OneOfBase<CustomerFound, CustomerNotFound>;

public readonly record struct CustomerFound(ReadModels.Customer Customer);

public readonly record struct CustomerNotFound(GetCustomerQuery Request);


public static class ReadModels
{
    public record Customer
    {
        public required Guid Id { get; init; }
        public required string TaxId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required DateOnly DateOfBirth { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }
        public required DateTimeOffset UpdatedAt { get; set; }
    }
}

[Mapper]
public static partial class ReadModelsMapper
{
    [MapperIgnoreSource(nameof(Customer.Name))]
    public static partial ReadModels.Customer ToReadModel(this Customer customer);
}