using Banking.Core.Customers;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Banking.Application.Features.Customers;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(ICustomerRepository repository, IUnitOfWorkFactory uowFactory, ILogger<CreateCustomerHandler> logger)
    {
        _repository = repository;
        _uowFactory = uowFactory;
        _logger = logger;
    }
    
    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Check if a customer with this taxId already exists
            var (taxId, firstName, lastName, dateOfBirth) = request;
            await using var uow = _uowFactory.Create();  
            var name = PersonName.From(firstName, lastName);
            var customerId = CustomerId.NewId();
            var customer = new Customer(customerId, taxId, name, dateOfBirth);
            await _repository.SaveAsync(customer, cancellationToken);
            await uow.CommitAsync(cancellationToken);
            return new CreateCustomerSuccess(customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return new CreateCustomerFailure(ex.Message);
        }
    }
}

public sealed record CreateCustomerCommand(
    string TaxId,
    string FirstName,
    string LastName,
    DateTimeOffset DateOfBirth)
    : IRequest<CreateCustomerResult>
{
    public void Deconstruct(out string taxId, out string firstName, out string lastName, out DateTimeOffset dateOfBirth)
    {
        taxId = TaxId;
        firstName = FirstName;
        lastName = LastName;
        dateOfBirth = DateOfBirth;
    }
}

[GenerateOneOf]
public partial class CreateCustomerResult : OneOfBase<CreateCustomerSuccess, CreateCustomerFailure>;

public readonly record struct CreateCustomerSuccess(Guid CustomerId);

public readonly record struct CreateCustomerFailure(string Description);