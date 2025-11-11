using System.Diagnostics.CodeAnalysis;

namespace Fireseal.Music.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record AlbumResponse(
    Guid Id, 
    string Title, 
    string Artist, 
    DateOnly ReleaseDate, 
    IReadOnlyCollection<AlbumTrackResponse> Tracks);