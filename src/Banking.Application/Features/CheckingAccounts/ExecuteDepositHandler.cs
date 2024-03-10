using Banking.Core.Accounts;
using Banking.Core.Transactions;
using MediatR;
using Microsoft.Extensions.Logging;
using NodaMoney;
using OneOf;

namespace Banking.Application.Features.CheckingAccounts;

public sealed class ExecuteDepositHandler : IRequestHandler<ExecuteDepositCommand, ExecuteDepositResult>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<ExecuteDepositHandler> _logger;

    public ExecuteDepositHandler(IAccountRepository accountRepository,
                                 ITransactionRepository transactionRepository,
                                 ILogger<ExecuteDepositHandler> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<ExecuteDepositResult> Handle(ExecuteDepositCommand request, CancellationToken cancellationToken)
    {
        var (accountId, amount, depositType) = request;
        await using var uow = _accountRepository.GetUnitOfWork();
        var accountLoad = await _accountRepository.LoadAsync(accountId, cancellationToken);

        ExecuteDepositResult result = null!;

        await accountLoad.Match(
            async account =>
            {
                var value = new Money(amount, account.Currency);
                var deposit = Deposit.NewDeposit(accountId, value, depositType);
                account.Deposit(deposit.Value);
                await _transactionRepository.SaveAsync(deposit, cancellationToken);
                await _accountRepository.SaveAsync(account, cancellationToken);
                result = new ExecuteDepositSuccess(deposit.Id);
            },
            () => Task.FromResult(new AccountNotFound(accountId)));

        await uow.CommitAsync(cancellationToken);

        return result;
    }
}

public record ExecuteDepositCommand(Guid AccountId, decimal Amount, DepositType DepositType) : IRequest<ExecuteDepositResult>
{
    public void Deconstruct(out Guid accountId, out decimal amount, out DepositType depositType)
    {
        accountId = AccountId;
        amount = Amount;
        depositType = DepositType;
    }
}

[GenerateOneOf]
public partial class ExecuteDepositResult : OneOfBase<ExecuteDepositSuccess, AccountNotFound>
{
}

public record ExecuteDepositSuccess(Guid DepositId);