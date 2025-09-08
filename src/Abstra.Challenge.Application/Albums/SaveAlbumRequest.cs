using System.Diagnostics.CodeAnalysis;

namespace Abstra.Challenge.Application.Albums;

[ExcludeFromCodeCoverage]
public abstract record SaveAlbumRequest(
    string Title, 
    string Artist, 
    DateOnly ReleaseDate);