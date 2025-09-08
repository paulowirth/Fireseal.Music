namespace Abstra.Challenge.Application.Albums;

public sealed record CreateAlbumRequest(
    string Title, 
    string Artist, 
    DateOnly ReleaseDate, 
    IReadOnlyCollection<CreateAlbumTrackRequest> Tracks) 
    : SaveAlbumRequest(Title, Artist, ReleaseDate);