using Banking.Application.EntityFramework;
using Banking.Core.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaMoney;
using OneOf;

namespace Banking.Application.Features.Accounts;

public class GetCustomerAccountsHandler : IRequestHandler<GetCustomerAccountsQuery, GetCustomerAccountsResult>
{
    private readonly BankingDbContext _dbContext;
    private readonly IAccountRepository _repository;
    private readonly IUnitOfWorkFactory _uowFactory;

    public GetCustomerAccountsHandler(BankingDbContext dbContext, IAccountRepository repository, IUnitOfWorkFactory uowFactory)
    {
        _dbContext = dbContext;
        _repository = repository;
        _uowFactory = uowFactory;
    }

    public async Task<GetCustomerAccountsResult> Handle(GetCustomerAccountsQuery request, CancellationToken cancellationToken)
    {
        await using var uow = _uowFactory.Create();

        // var customerExists = await _dbContext.Customers
        //                                      .AnyAsync(x => (Guid)x.Id == request.CustomerId, cancellationToken);
        //
        // if (!customerExists)
        //     return new RecordNotFound<Guid>(request.CustomerId);

        var accountExists = await ExistsAccountFor(request.CustomerId, cancellationToken);

        var records = accountExists
                          ? await GetAccountsForAsync(request.CustomerId, cancellationToken)
                          : await CreateAccountForAsync(request.CustomerId, cancellationToken);

        await uow.CommitAsync(cancellationToken);
        return new GetCustomerAccountsSuccess(records);
    }

    private async Task<IEnumerable<ReadModels.CheckingAccount>> CreateAccountForAsync(Guid customerId, CancellationToken cancellationToken)
    {
        const decimal totalLimit = 10_000;
        const string bankBranch = "0001";
        var limit = new Money(totalLimit, Currency.FromCode("BRL"));
        var account = CheckingAccount.NewCheckingAccount(customerId, bankBranch, limit);
        await _repository.SaveAsync(account, cancellationToken);
        var model = account.ToModel();
        return [model];
    }

    private async Task<bool> ExistsAccountFor(Guid customerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
                               .AnyAsync(x => (Guid)x.CustomerId == customerId, cancellationToken);
    }

    private async Task<IEnumerable<ReadModels.CheckingAccount>> GetAccountsForAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var records = await _dbContext.Accounts
                                      .OfType<CheckingAccount>()
                                      .Where(x => (Guid)x.CustomerId == customerId)
                                      .OrderBy(x => x.BankBranch)
                                      .ThenBy(x => x.AccountNumber)
                                      .Select(x => x.ToModel())
                                      .ToListAsync(cancellationToken);
        return records;
    }
}

public readonly record struct GetCustomerAccountsQuery(Guid CustomerId) : IRequest<GetCustomerAccountsResult>;

[GenerateOneOf]
public partial class GetCustomerAccountsResult : OneOfBase<GetCustomerAccountsSuccess, RecordNotFound<Guid>>;

public record GetCustomerAccountsSuccess(IEnumerable<ReadModels.CheckingAccount> Records);