using System.Diagnostics.CodeAnalysis;

namespace Fireseal.Music.Domain.Abstractions;

[ExcludeFromCodeCoverage]
public abstract class Entity<TKey> : IEntity<TKey>
{
    public required TKey Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = TimeProvider.System.GetUtcNow().DateTime;
    
    public DateTime UpdatedAt { get; set; } = TimeProvider.System.GetUtcNow().DateTime;
}