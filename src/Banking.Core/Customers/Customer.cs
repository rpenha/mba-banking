namespace Banking.Core.Customers;

public class Customer : AggregateRoot<CustomerId>
{
    private Customer()
    {
    }

    public Customer(CustomerId id, TaxId taxId, PersonName name, DateOfBirth dateOfBirth)
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