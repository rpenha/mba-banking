using Banking.Application.Features;
using Banking.Application.Features.Customers;
using MediatR;

namespace Banking.Api;

public static class CustomerEndpoints
{
    public static RouteGroupBuilder MapCustomerApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateCustomer)
             .Produces(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/{customerId:guid}", GetCustomer)
             .Produces<ReadModels.Customer>()
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);
        
        return group;
    }

    private static async Task<IResult> CreateCustomer(CreateCustomerCommand command,
                                                      IMediator mediator,
                                                      HttpContext context,
                                                      CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        var requestPath = context.Request.Path;

        return result.Match<IResult>(
            _ => TypedResults.Created($"{requestPath}/{command.CustomerId}"),
            failure => TypedResults.Problem(title: failure.Description)
        );
    }

    private static async Task<IResult> GetCustomer(Guid customerId,
                                                   IMediator mediator,
                                                   CancellationToken cancellationToken)
    {
        var query = new GetCustomerQuery(customerId);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match<IResult>(TypedResults.Ok, _ => TypedResults.NotFound());
    }
}