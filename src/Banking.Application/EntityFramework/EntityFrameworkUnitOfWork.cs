using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Banking.Application.EntityFramework;

public sealed class EntityFrameworkUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IDbContextTransaction _transaction;
    private readonly ActivitySource _activitySource;
    private bool _committed = false;

    public EntityFrameworkUnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
        _activitySource = Activity.Current?.Source ?? new ActivitySource(nameof(EntityFrameworkUnitOfWork));
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        using var _ = _activitySource.StartActivity();
        
        if (_committed)
            throw new InvalidOperationException("Transaction already committed.");
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        
        _committed = true;
    }
    
    public async ValueTask DisposeAsync()
    {
        await _transaction.DisposeAsync();
        await _dbContext.DisposeAsync();
    }
}

public sealed class EntityFrameworkUnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public EntityFrameworkUnitOfWorkFactory(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IUnitOfWork Create() => new EntityFrameworkUnitOfWork(_dbContext);
}