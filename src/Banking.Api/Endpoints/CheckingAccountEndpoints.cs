using Banking.Application.Features.Customers;
using MediatR;

namespace Banking.Api.Endpoints;

public static class CheckingAccountsEndpoints
{
    public static RouteGroupBuilder MapChekingAccountApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateCheckingAccount)
             .Produces(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status500InternalServerError);

        group.WithTags("Checking Accounts");
        
        return group;
    }
    
    private static async Task<IResult> CreateCheckingAccount(CreateCheckingAccountCommand command,
                                                             IMediator mediator,
                                                             HttpContext context,
                                                             CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        var requestPath = context.Request.Path;

        return result.Match<IResult>(
            success => TypedResults.Created($"{requestPath}/{success.AccountId}"),
            failure => TypedResults.Problem(title: failure.Description)
        );
    }
}