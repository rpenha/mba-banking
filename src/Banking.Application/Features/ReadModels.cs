using Banking.Core.Transactions;
using NodaMoney;

namespace Banking.Application.Features;

public static class ReadModels
{
    public record Customer
    {
        public required Guid Id { get; init; }
        public required string TaxId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required DateOnly DateOfBirth { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }
        public required DateTimeOffset UpdatedAt { get; init; }
    }
    
    public record Transaction
    {
        public required Guid Id { get; init; }
        public required string Description { get; init; }
        public required DateTimeOffset Timestamp { get; init; }
        public required string Currency { get; init; }
        public required decimal Amount { get; init; }
        public required TransactionDirection Direction { get; init; }
    }
}