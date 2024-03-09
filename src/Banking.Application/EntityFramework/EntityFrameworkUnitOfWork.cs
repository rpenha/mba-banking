using System.Diagnostics;
using Banking.Core;
using Microsoft.EntityFrameworkCore;

namespace Banking.Application.EntityFramework;

public sealed class EntityFrameworkUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly ActivitySource _activitySource;

    public EntityFrameworkUnitOfWork(DbContext dbContext, ActivitySource activitySource)
    {
        _dbContext = dbContext;
        _activitySource = activitySource;
    }

    public ValueTask DisposeAsync()
    {
        return _dbContext.DisposeAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        using var _ = _activitySource.StartActivity();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}