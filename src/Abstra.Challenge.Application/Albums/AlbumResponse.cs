using System.Diagnostics.CodeAnalysis;

namespace Abstra.Challenge.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record AlbumResponse(
    Guid Id, 
    string Title, 
    string Artist, 
    DateOnly ReleaseDate, 
    IReadOnlyCollection<AlbumTrackResponse> Tracks);