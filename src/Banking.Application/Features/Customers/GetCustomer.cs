using Banking.Application.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Banking.Application.Features.Customers;

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
                                     .Where(x => (Guid)x.Id == request.CustomerId)
                                     .Select(x => x.ToCustomerModel())
                                     .FirstOrDefaultAsync(cancellationToken);

        return result switch
               {
                   not null => new RecordFound<ReadModels.Customer>(result),
                   _ => new CustomerNotFound(request)
               };
    }
}

public readonly record struct GetCustomerQuery(Guid CustomerId) : IRequest<GetCustomerResult>;

[GenerateOneOf]
public partial class GetCustomerResult : OneOfBase<RecordFound<ReadModels.Customer>, CustomerNotFound>;

public readonly record struct CustomerNotFound(GetCustomerQuery Request);