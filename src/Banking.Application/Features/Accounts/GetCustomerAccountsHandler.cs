using Banking.Application.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Banking.Application.Features.Accounts;

public class GetCustomerAccountsHandler : IRequestHandler<GetCustomerAccountsQuery, GetCustomerAccountsResult>
{
    private readonly BankingDbContext _dbContext;

    public GetCustomerAccountsHandler(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetCustomerAccountsResult> Handle(GetCustomerAccountsQuery request, CancellationToken cancellationToken)
    {
        var customerExists = await _dbContext.Customers
                                             .AnyAsync(x => (Guid)x.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
            return new RecordNotFound<Guid>(request.CustomerId);

        var records = await _dbContext.Accounts
                                      .Where(x => (Guid)x.CustomerId == request.CustomerId)
                                      .Select(x => x.ToCustomerAccountModel())
                                      .ToListAsync(cancellationToken);

        return new GetCustomerAccountsSuccess(records);
    }
}

public readonly record struct GetCustomerAccountsQuery(Guid CustomerId) : IRequest<GetCustomerAccountsResult>;

[GenerateOneOf]
public partial class GetCustomerAccountsResult : OneOfBase<GetCustomerAccountsSuccess, RecordNotFound<Guid>>;

public record GetCustomerAccountsSuccess(IEnumerable<ReadModels.CustomerAccount> Records);