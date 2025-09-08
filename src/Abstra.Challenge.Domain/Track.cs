using System.Diagnostics.CodeAnalysis;
using Abstra.Challenge.Domain.Abstractions;

namespace Abstra.Challenge.Domain;

[ExcludeFromCodeCoverage]
public sealed class Track : Entity<Guid>
{
    public Guid AlbumId { get; set; }
    
    public required string Isrc { get; set; }
    
    public required string Title { get; set; }
    
    public required TimeSpan Duration { get; set; }
    
    public Album? Album { get; set; }
}