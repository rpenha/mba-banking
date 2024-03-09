using MediatR;

namespace Banking.Application.Features.CreateCustomer;

public record CreateCustomerCommand(Guid CustomerId, string TaxId, string FirstName, string LastName, DateTimeOffset DateOfBirth)
    : IRequest<CommandResult>
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