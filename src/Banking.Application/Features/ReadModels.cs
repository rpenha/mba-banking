using Banking.Core.Transactions;

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

    // public record CustomerAccount
    // {
    //     public required Guid Id { get; init; }
    //     public required string BankBranch { get; init; }
    //     public required string AccountNumber { get; init; }
    //     public required string Currency { get; init; }
    //     public required DateTimeOffset CreatedAt { get; init; }
    //     public required DateTimeOffset UpdatedAt { get; init; }
    // }

    public record CheckingAccount
    {
        public required Guid Id { get; init; }
        public required Guid CustomerId { get; init; }
        public required string BankBranch { get; init; }
        public required string AccountNumber { get; init; }
        public required string Currency { get; init; }
        public required decimal Balance { get; init; }
        public decimal AvailableAmount => Balance + CurrentLimit;
        public required decimal TotalLimit { init; get; }
        public required decimal CurrentLimit { get; init; }
        public required decimal UsedLimit { init; get; }
        public required bool IsUsingLimit { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }
        public required DateTimeOffset UpdatedAt { get; init; }
    }
}