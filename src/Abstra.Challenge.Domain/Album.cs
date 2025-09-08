using System.Diagnostics.CodeAnalysis;
using Abstra.Challenge.Domain.Abstractions;

namespace Abstra.Challenge.Domain;

[ExcludeFromCodeCoverage]
public sealed class Album : Entity<Guid>
{
    public required string Title { get; set; }
    
    public required string Artist { get; set; }
    
    public required DateOnly ReleaseDate { get; set; }

    public required List<Track> Tracks { get; set; } = [];
}