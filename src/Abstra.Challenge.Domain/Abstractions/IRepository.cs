using System.Linq.Expressions;

namespace Abstra.Challenge.Domain.Abstractions;

public interface IRepository<TEntity, in TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<TEntity?> Get(TKey entityKey,
        Expression<Func<TEntity, object?>>[] includes,
        CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<TEntity>> List(
        Expression<Func<TEntity, object?>>[] includes, 
        CancellationToken cancellationToken);
    
    Task Insert(TEntity entity, CancellationToken cancellationToken);

    Task<bool> Update(TEntity entity, Expression<Func<TEntity, object?>>[] navigationProperties,
        CancellationToken cancellationToken);
    
    Task<bool> Delete(TKey entityKey, CancellationToken cancellationToken);

    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
}
