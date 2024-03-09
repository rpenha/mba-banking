using Banking.Application.Features;
using Banking.Application.Features.CreateCustomer;
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
        
        return group;
    }

    private static async Task<IResult> CreateCustomer(CreateCustomerCommand command,
                                                      IMediator mediator,
                                                      HttpContext context,
                                                      CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        var requestPath = context.Request.Path;
        
        return result switch
               {
                   Success => TypedResults.Created($"{requestPath}/{command.CustomerId}"),
                   Failure failure => TypedResults.Problem(title: failure.Message,
                                                           detail: string.Join(", ", failure.Errors)),
                   _ => TypedResults.Problem("Cannot process request")
               };
    }
}