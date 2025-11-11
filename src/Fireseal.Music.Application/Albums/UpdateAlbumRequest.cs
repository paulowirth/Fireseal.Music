namespace Fireseal.Music.Application.Albums;

public sealed record UpdateAlbumRequest(string Title, string Artist, DateOnly ReleaseDate) 
    : SaveAlbumRequest(Title, Artist, ReleaseDate);