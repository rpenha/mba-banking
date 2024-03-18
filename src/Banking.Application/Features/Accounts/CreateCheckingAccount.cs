using Banking.Core.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;
using NodaMoney;
using OneOf;

namespace Banking.Application.Features.Accounts;

public sealed class CreateCheckingAccountHandler : IRequestHandler<CreateCheckingAccountCommand, CreateCheckingAccountResult>
{
    private readonly IAccountRepository _repository;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ILogger<CreateCheckingAccountHandler> _logger;

    public CreateCheckingAccountHandler(IAccountRepository repository, IUnitOfWorkFactory uowFactory, ILogger<CreateCheckingAccountHandler> logger)
    {
        _repository = repository;
        _uowFactory = uowFactory;
        _logger = logger;
    }

    public async Task<CreateCheckingAccountResult> Handle(CreateCheckingAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (customerId, bankBranch, totalLimit) = request;
            var limit = new Money(totalLimit, Currency.FromCode("BRL"));
            await using var uow = _uowFactory.Create();
            var account = CheckingAccount.NewCheckingAccount(customerId, bankBranch, limit);
            await _repository.SaveAsync(account, cancellationToken);
            await uow.CommitAsync(cancellationToken);
            return new CreateCheckingAccountSuccess(account.Id,
                                                    account.CustomerId,
                                                    account.BankBranch,
                                                    account.AccountNumber,
                                                    account.Balance.Amount,
                                                    account.TotalLimit.Amount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return new CreateCheckingAccountFailure(ex.Message);
        }
    }
}

public record CreateCheckingAccountCommand(Guid CustomerId, string BankBranch, decimal TotalLimit)
    : IRequest<CreateCheckingAccountResult>
{
    public void Deconstruct(out Guid customerId, out string bankBranch, out decimal totalLimit)
    {
        customerId = CustomerId;
        bankBranch = BankBranch;
        totalLimit = TotalLimit;
    }
}

[GenerateOneOf]
public partial class CreateCheckingAccountResult : OneOfBase<CreateCheckingAccountSuccess, CreateCheckingAccountFailure>;

public readonly record struct CreateCheckingAccountSuccess(
    Guid AccountId,
    Guid CustomerId,
    string BankBranch,
    string AccountNumber,
    decimal CurrentBalance,
    decimal TotalLimit);

public readonly record struct CreateCheckingAccountFailure(string Description);