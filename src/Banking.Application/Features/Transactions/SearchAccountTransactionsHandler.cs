using Banking.Application.EntityFramework;
using Banking.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Banking.Application.Features.Transactions;

public class SearchAccountTransactionsHandler : IRequestHandler<SearchAccountTransactionsQuery, SearchAccountTransactionsResult>
{
    private readonly BankingDbContext _dbContext;

    public SearchAccountTransactionsHandler(BankingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SearchAccountTransactionsResult> Handle(SearchAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var (accountId, currentPage, pageSize, skip, startDate, endDate) = request;

        var accountExists = await _dbContext.Accounts
                                            .AnyAsync(x => (Guid)x.Id == accountId, cancellationToken);

        if (!accountExists) return new RecordNotFound<Guid>(accountId);

        var totalRecords = await _dbContext.Transactions
                                           .CountAsync(x => (Guid)x.AccountId == accountId
                                                            && startDate <= x.Timestamp
                                                            && endDate >= x.Timestamp, cancellationToken);

        var records = await _dbContext.Transactions
                                      .Where(x => (Guid)x.AccountId == accountId
                                                  && startDate <= x.Timestamp
                                                  && endDate >= x.Timestamp)
                                      .Skip(skip)
                                      .Take(pageSize)
                                      .Select(x => x.ToTransactionModel())
                                      .ToArrayAsync(cancellationToken);

        return new SearchAccountTransactionsSuccess(records, currentPage, pageSize, totalRecords);
    }
}

[GenerateOneOf]
public partial class SearchAccountTransactionsResult : OneOfBase<SearchAccountTransactionsSuccess, RecordNotFound<Guid>>;

public sealed record SearchAccountTransactionsSuccess(IEnumerable<ReadModels.Transaction> Records, int CurrentPage, int PageSize, int TotalRecords)
    : PagedResult<ReadModels.Transaction>(Records, CurrentPage, PageSize, TotalRecords);

public sealed record SearchAccountTransactionsQuery : IRequest<SearchAccountTransactionsResult>
{
    public Guid AccountId { get; init; }

    public int Page { get; init; }

    public int Size { get; init; }

    public DateOnly? Start { get; init; } = DateOnly.FromDateTime(DateTimeProvider.Now.DateTime);

    public DateOnly? End { get; init; } = DateOnly.FromDateTime(DateTimeProvider.Now.DateTime);

    public int Skip => Page * Size;

    public void Deconstruct(out Guid accountId, out int page, out int size, out int skip, out DateTime start, out DateTime end)
    {
        accountId = AccountId;
        page = Page;
        size = Size;
        skip = Skip;
        start = Start!.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local).ToUniversalTime();
        end = End!.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Local).ToUniversalTime();
    }
}