using Banking.Core.Customers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Banking.Application.Features.CreateCustomer;

public sealed class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CommandResult>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(ICustomerRepository repository, ILogger<CreateCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<CommandResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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
            return new Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return new Failure { Message = ex.Message };
        }
    }
}