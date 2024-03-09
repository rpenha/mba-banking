namespace Banking.Core.Customers;

public class Customer : AggregateRoot<Guid>
{
    private Customer()
    {
    }

    public Customer(Guid id, TaxId taxId, PersonName name, DateOfBirth dateOfBirth)
        : base(id)
    {
        TaxId = taxId;
        Name = name;
        DateOfBirth = dateOfBirth;
    }

    public TaxId TaxId { get; private init; }
    public PersonName Name { get; private init; }
    public string FirstName => Name.FirstName;
    public string LastName => Name.LastName;
    public DateOfBirth DateOfBirth { get; private init; }
    //public Address Address { get; private init; }
}