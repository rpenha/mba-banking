using Banking.Application.EntityFramework;
using Banking.Core.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Banking.Application.Features.Accounts;

public sealed class GetAccountHandler : IRequestHandler<GetAccountQuery, GetAccountResult>
{
    private readonly BankingDbContext _dbContext;

    public GetAccountHandler(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAccountResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Accounts
                                     .OfType<CheckingAccount>()
                                     .Where(x => (Guid)x.Id == request.AccountId)
                                     .Select(x => x.ToModel())
                                     .FirstOrDefaultAsync(cancellationToken);
        
        return result switch
               {
                   not null => new RecordFound<ReadModels.CheckingAccount>(result),
                   _ => new RecordNotFound<Guid>(request.AccountId)
               };
    }
}

public record GetAccountQuery(Guid AccountId) : IRequest<GetAccountResult>;

[GenerateOneOf]
public partial class GetAccountResult : OneOfBase<RecordFound<ReadModels.CheckingAccount>, RecordNotFound<Guid>>;