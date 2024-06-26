using Banking.Application.Features;
using Banking.Application.Features.Accounts;
using Banking.Application.Features.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Endpoints;

public static class CustomersEndpoints
{
    public static RouteGroupBuilder MapCustomersApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateCustomer)
             .Produces(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/{customerId:guid}", GetCustomer)
             .Produces<RecordFound<ReadModels.Customer>>()
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

        group.MapPost("/{customerId:guid}/accounts", CreateCheckingAccount)
             .Produces(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Accounts");
        
        group.MapGet("/{customerId:guid}/accounts", GetCustomerAccounts)
             .Produces<GetCustomerAccountsSuccess>()
             .Produces(StatusCodes.Status401Unauthorized)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Accounts");

        //group.WithTags("Customers");
        
        group.WithTags("Customers")
             .RequireAuthorization();

        return group;
    }
    
    private static async Task<IResult> GetCustomerAccounts([FromRoute] Guid customerId,
                                                           IMediator mediator,
                                                           CancellationToken cancellationToken = default)
    {
        var query = new GetCustomerAccountsQuery(customerId);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IResult>(
            TypedResults.Ok,
            _ => TypedResults.NotFound());
    }

    private static async Task<IResult> CreateCustomer(CreateCustomerCommand command,
                                                      IMediator mediator,
                                                      HttpContext context,
                                                      CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        var requestPath = context.Request.Path;

        return result.Match<IResult>(
            success => TypedResults.Created($"{requestPath}/{success.CustomerId}"),
            failure => TypedResults.Problem(title: failure.Description)
        );
    }

    private static async Task<IResult> GetCustomer(Guid customerId,
                                                   IMediator mediator,
                                                   CancellationToken cancellationToken)
    {
        var query = new GetCustomerQuery(customerId);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IResult>(
            TypedResults.Ok,
            _ => TypedResults.NotFound());
    }

    private static async Task<IResult> CreateCheckingAccount([FromRoute] Guid customerId,
                                                             [FromBody] CreateCheckingAccountBody body,
                                                             IMediator mediator,
                                                             HttpContext context,
                                                             CancellationToken cancellationToken = default)
    {
        var (bankBranch, totalLimit) = body;
        var cmd = new CreateCheckingAccountCommand(customerId, bankBranch, totalLimit);
        var result = await mediator.Send(cmd, cancellationToken);
        var requestPath = context.Request.Path;

        return result.Match<IResult>(
            success => TypedResults.Created($"/api/accounts/{success.AccountId}"),
            failure => TypedResults.Problem(title: failure.Description)
        );
    }
}

public readonly record struct CreateCheckingAccountBody(string BankBranch, decimal TotalLimit)
{
    public void Deconstruct(out string bankBranch, out decimal totalLimit)
    {
        bankBranch = BankBranch;
        totalLimit = TotalLimit;
    }
}