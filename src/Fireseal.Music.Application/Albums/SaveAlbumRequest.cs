using System.Diagnostics.CodeAnalysis;

namespace Fireseal.Music.Application.Albums;

[ExcludeFromCodeCoverage]
public abstract record SaveAlbumRequest(
    string Title, 
    string Artist, 
    DateOnly ReleaseDate);