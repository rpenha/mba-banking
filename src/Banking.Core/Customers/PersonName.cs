namespace Banking.Core.Customers;

public readonly record struct PersonName
{
    public PersonName(string firstName, string lastName)
        : this(new NormalizedString(firstName), new NormalizedString(lastName))
    {
        
    }
    
    public PersonName(NormalizedString firstName, NormalizedString lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonName From(string firstName, string lastName)
    {
        return new PersonName(firstName, lastName);
    }

    public string FirstName { get; private init; }
    
    public string LastName { get; private init;  }

    public override string ToString() => string.Join(" ", FirstName, LastName);
}