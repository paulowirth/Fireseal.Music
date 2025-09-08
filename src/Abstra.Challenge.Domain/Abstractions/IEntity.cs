namespace Abstra.Challenge.Domain.Abstractions;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    
    DateTime CreatedAt { get; set; }
    
    DateTime UpdatedAt { get; set; }
}