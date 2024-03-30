using Banking.Application.Features;
using Banking.Application.Features.Accounts;
using Banking.Application.Features.Transactions;
using Banking.Core.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Endpoints;

public static class AccountsEndpoints
{
    public static RouteGroupBuilder MapTransactionsApi(this RouteGroupBuilder group)
    {
        group.MapPost("/{accountId:guid}/deposits", ExecuteDeposit)
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status422UnprocessableEntity)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Transactions");;

        group.MapPost("/{accountId:guid}/withdrawals", ExecuteWithdraw)
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status422UnprocessableEntity)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Transactions");;

        group.MapGet("/{accountId:guid}/transactions", SearchAccountTransactions)
             .Produces<SearchAccountTransactionsResponse>()
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Transactions");
        
        group.MapGet("/{accountId:guid}", GetAccount)
             .Produces<RecordFound<ReadModels.CheckingAccount>>()
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Accounts");

        group.RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetAccount([FromRoute] Guid accountId,
                                                  IMediator mediator,
                                                  CancellationToken cancellationToken = default)
    {
        var query = new GetAccountQuery(accountId);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IResult>(
            TypedResults.Ok,
            _ => TypedResults.NotFound());
    }

    private static async Task<IResult> ExecuteDeposit([FromRoute] Guid accountId,
                                                      [FromBody] ExecuteDeposityBody body,
                                                      IMediator mediator,
                                                      CancellationToken cancellationToken = default)
    {
        var (amount, deposityType) = body;
        var cmd = new ExecuteDepositCommand(accountId, amount, deposityType);
        var result = await mediator.Send(cmd, cancellationToken);

        return result.Match<IResult>(
            _ => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        );
    }

    private static async Task<IResult> ExecuteWithdraw([FromRoute] Guid accountId,
                                                       [FromBody] ExecuteWithdrawBody body,
                                                       IMediator mediator,
                                                       CancellationToken cancellationToken = default)
    {
        var (amount, withdrawType) = body;
        var cmd = new ExecuteWithdrawCommand(accountId, amount, withdrawType);
        var result = await mediator.Send(cmd, cancellationToken);

        return result.Match<IResult>(
            _ => TypedResults.NoContent(),
            limit => TypedResults.UnprocessableEntity(limit.Description),
            _ => TypedResults.NotFound()
        );
    }

    private static async Task<IResult> SearchAccountTransactions([AsParameters] SearchAccountTransactionsQuery query,
                                                                 IMediator mediator,
                                                                 HttpContext context,
                                                                 CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IResult>(
            success => TypedResults.Ok(SearchAccountTransactionsResponse.From(context.Request, success)),
            _ => TypedResults.NotFound()
        );
    }
}

public readonly record struct ExecuteDeposityBody
{
    public decimal Amount { get; init; }

    public DepositType DepositType { get; init; }

    public void Deconstruct(out decimal amount, out DepositType depositType)
    {
        amount = Amount;
        depositType = DepositType;
    }
}

public readonly record struct ExecuteWithdrawBody
{
    public decimal Amount { get; init; }

    public WithdrawType WithdrawType { get; init; }

    public void Deconstruct(out decimal amount, out WithdrawType withdrawType)
    {
        amount = Amount;
        withdrawType = WithdrawType;
    }
}

public record SearchAccountTransactionsResponse : PagedResult<ReadModels.Transaction>
{
    public static SearchAccountTransactionsResponse From(HttpRequest request, PagedResult<ReadModels.Transaction> inner)
    {
        var (transactions, currentPage, pageSize, totalRecords) = inner;
        return new SearchAccountTransactionsResponse(request, transactions, currentPage, pageSize, totalRecords);
    }

    private SearchAccountTransactionsResponse(HttpRequest request,
                                              IEnumerable<ReadModels.Transaction> records,
                                              int currentPage,
                                              int pageSize,
                                              int totalRecords)
        : base(records, currentPage, pageSize, totalRecords)
    {
        ApiLink[] links =
        [
            CreatePageLink(request, "first-page", 0),
            CreatePageLink(request, "last-page", TotalPages - 1),
        ];

        if (!FirstPage)
            links = [..links, CreatePageLink(request, "previous-page", CurrentPage - 1)];

        if (!LastPage)
            links = [..links, CreatePageLink(request, "next-page", CurrentPage + 1)];

        Links = links;
    }

    private static ApiLink CreatePageLink(HttpRequest request, string rel, int page)
    {
        var temp = request.Query.ToDictionary();
        temp["currentPage"] = page.ToString();
        var qs = QueryString.Create(temp);
        return new ApiLink(rel, $"{request.Path}{qs}");
    }

    public ApiLink[] Links { get; private init; }
}

public readonly record struct ApiLink(string Rel, string Href);