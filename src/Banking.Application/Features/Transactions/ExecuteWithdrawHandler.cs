using Banking.Core;
using Banking.Core.Accounts;
using Banking.Core.Transactions;
using MediatR;
using Microsoft.Extensions.Logging;
using NodaMoney;
using OneOf;

namespace Banking.Application.Features.Transactions;

public sealed class ExecuteWithdrawHandler : IRequestHandler<ExecuteWithdrawCommand, ExecuteWithdrawResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ILogger<ExecuteWithdrawHandler> _logger;

    public ExecuteWithdrawHandler(IAccountRepository accountRepository,
                                  ITransactionRepository transactionRepository,
                                  IUnitOfWorkFactory uowFactory,
                                  ILogger<ExecuteWithdrawHandler> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _uowFactory = uowFactory;
        _logger = logger;
    }

    public async Task<ExecuteWithdrawResult> Handle(ExecuteWithdrawCommand request, CancellationToken cancellationToken)
    {
        var (accountId, amount, depositType) = request;
        await using var uow = _uowFactory.Create();
        var accountLoad = await _accountRepository.LoadAsync(accountId, cancellationToken);

        ExecuteWithdrawResult result = null!;

        await accountLoad.Match<Task>(
            async account =>
            {
                var value = new Money(amount, account.Currency);
                var withdraw = Withdraw.NewWithdraw(accountId, value, depositType);
                var success = account.Withdraw(withdraw.Value);
                if (!success)
                {
                    result = new InsufficientLimit(accountId);
                    return;
                }

                await _transactionRepository.SaveAsync(withdraw, cancellationToken);
                await _accountRepository.SaveAsync(account, cancellationToken);
                result = new ExecuteWithdrawSuccess(withdraw.Id);
            },
            () => Task.FromResult(new RecordNotFound<Guid>(accountId)));

        await uow.CommitAsync(cancellationToken);

        return result;
    }
}

public record ExecuteWithdrawCommand(Guid AccountId, decimal Amount, WithdrawType WithdrawType) : IRequest<ExecuteWithdrawResult>
{
    public void Deconstruct(out Guid accountId, out decimal amount, out WithdrawType depositType)
    {
        accountId = AccountId;
        amount = Amount;
        depositType = WithdrawType;
    }
}

[GenerateOneOf]
public partial class ExecuteWithdrawResult : OneOfBase<ExecuteWithdrawSuccess, InsufficientLimit, RecordNotFound<Guid>>
{
}

public record ExecuteWithdrawSuccess(Guid DepositId);

public record InsufficientLimit(Guid AccountId)
{
    public string Description => $"Insufficient funds to complete withdrawal for account {AccountId}.";
}