using System.Diagnostics;
using Banking.Core;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Banking.Application.EntityFramework;

public abstract class EntityFrameworkRespository<TId, TAggregateRoot, TDbContext>
    : IRepository<TId, TAggregateRoot>, IAsyncDisposable
    where TId : IEquatable<TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    private readonly ActivitySource _activitySource;
    private readonly EntityFrameworkUnitOfWork _uow;

    protected EntityFrameworkRespository(TDbContext dbContext)
    {
        _dbContext = dbContext;
        _activitySource = Activity.Current?.Source ?? new ActivitySource(nameof(EntityFrameworkRespository<TId, TAggregateRoot, TDbContext>));;
        _uow = new EntityFrameworkUnitOfWork(dbContext);
    }

    private DbSet<TAggregateRoot> Collection => _dbContext.Set<TAggregateRoot>();

    private bool Exists(TAggregateRoot aggregate) => Collection.Any(x => x.Id.Equals(aggregate.Id));

    public async Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        using var _ = _activitySource.StartActivity();
        if (Exists(aggregate))
        {
            await Task.FromResult(Collection.Update(aggregate));
        }
        else
        {
            await Collection.AddAsync(aggregate, cancellationToken);
        }
    }

    public async Task<Option<TAggregateRoot>> LoadAsync(TId id, CancellationToken cancellationToken = default)
    {
        using var _ = _activitySource.StartActivity();
        return await Collection.FindAsync([id], cancellationToken: cancellationToken);
    }

    public IUnitOfWork GetUnitOfWork() => _uow;

    public async ValueTask DisposeAsync() => await _uow.DisposeAsync();
}