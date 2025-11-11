namespace Fireseal.Music.Domain.Abstractions;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    
    DateTime CreatedAt { get; set; }
    
    DateTime UpdatedAt { get; set; }
}