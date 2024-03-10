using Banking.Application.Features.CheckingAccounts;
using Banking.Core.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Endpoints;

public static class AccountsEndpoints
{
    public static RouteGroupBuilder MapAccountsApi(this RouteGroupBuilder group)
    {
        // group.MapPost("/", CreateCheckingAccount)
        //      .Produces(StatusCodes.Status201Created)
        //      .Produces(StatusCodes.Status400BadRequest)
        //      .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/{accountId:guid}/deposits", ExecuteDeposit)
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status422UnprocessableEntity)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Transactions");

        group.MapPost("/{accountId:guid}/withdrawals", ExecuteWithdraw)
             .Produces(StatusCodes.Status204NoContent)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status422UnprocessableEntity)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Transactions");

        group.WithTags("Checking Accounts");

        return group;
    }

    // private static async Task<IResult> CreateCheckingAccount(CreateCheckingAccountCommand command,
    //                                                          IMediator mediator,
    //                                                          HttpContext context,
    //                                                          CancellationToken cancellationToken = default)
    // {
    //     var result = await mediator.Send(command, cancellationToken);
    //
    //     var requestPath = context.Request.Path;
    //
    //     return result.Match<IResult>(
    //         success => TypedResults.Created($"{requestPath}/{success.AccountId}"),
    //         failure => TypedResults.Problem(title: failure.Description)
    //     );
    // }

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