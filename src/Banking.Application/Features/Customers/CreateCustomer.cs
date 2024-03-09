using Banking.Core.Customers;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Banking.Application.Features.Customers;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(ICustomerRepository repository, ILogger<CreateCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Check if a customer with this taxId already exists
            var (customerId, taxId, firstName, lastName, dateOfBirth) = request;
            await using var uow = _repository.GetUnitOfWork();
            var name = PersonName.From(firstName, lastName);
            var customer = new Customer(customerId, taxId, name, dateOfBirth);
            await _repository.SaveAsync(customer, cancellationToken);
            await uow.CommitAsync(cancellationToken);
            return new CreateCustomerSuccess();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return new CreateCustomerFailure(ex.Message);
        }
    }
}

public sealed record CreateCustomerCommand(Guid CustomerId, string TaxId, string FirstName, string LastName, DateTimeOffset DateOfBirth)
    : IRequest<CreateCustomerResult>
{
    public void Deconstruct(out Guid customerId, out string taxId, out string firstName, out string lastName, out DateTimeOffset dateOfBirth)
    {
        customerId = CustomerId;
        taxId = TaxId;
        firstName = FirstName;
        lastName = LastName;
        dateOfBirth = DateOfBirth;
    }
}

[GenerateOneOf]
public partial class CreateCustomerResult : OneOfBase<CreateCustomerSuccess, CreateCustomerFailure>;

public readonly record struct CreateCustomerFailure(string Description);

public readonly record struct CreateCustomerSuccess;