using System.Linq.Expressions;
using Fireseal.Music.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fireseal.Music.Infrastructure.Abstractions.Persistence;

internal abstract class DbContextRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    protected DbContextRepository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var existsResult =
            _dbSet
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken);

        return existsResult;
    }

    public async Task<TEntity?> Get(TKey entityKey,
        Expression<Func<TEntity, object?>>[] includes,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        var getResult =
            await query
                .Where(entity => entity.Id.Equals(entityKey))
                .SingleOrDefaultAsync(cancellationToken);

        return getResult;
    }

    public async Task<IReadOnlyCollection<TEntity>> List(Expression<Func<TEntity, object?>>[] includes,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        var listResult =
            await query
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return listResult;
    }

    public async Task Insert(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> Update(
        TEntity entity, Expression<Func<TEntity, object?>>[] includes, CancellationToken cancellationToken)
    {
        var entityToUpdate = await Get(entity.Id, includes, cancellationToken);

        if (entityToUpdate is null)
            return false;

        var originalEntityEntry = _context.Entry(entityToUpdate);

        if (originalEntityEntry.State == EntityState.Detached)
            _dbSet.Attach(entityToUpdate);

        originalEntityEntry.CurrentValues.SetValues(entity);

        var updateResult = await _context.SaveChangesAsync(cancellationToken);
        _context.ChangeTracker.Clear();

        return updateResult > 0;
    }

    public async Task<bool> Delete(TKey entityKey, CancellationToken cancellationToken)
    {
        var getResult = await Get(entityKey, [], cancellationToken);

        if (getResult is null)
            return false;

        _dbSet.Remove(getResult);

        return await _context.SaveChangesAsync(cancellationToken) == 1;
    }
}