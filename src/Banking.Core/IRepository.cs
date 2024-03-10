using LanguageExt;

namespace Banking.Core;

public interface IRepository<in TId, TAggregateRoot>
    where TId : IEquatable<TId>
    where TAggregateRoot : IAggregateRoot<TId>
{
    Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);

    Task<Option<TAggregateRoot>> LoadAsync(TId id, CancellationToken cancellationToken = default);

    IUnitOfWork GetUnitOfWork();
}