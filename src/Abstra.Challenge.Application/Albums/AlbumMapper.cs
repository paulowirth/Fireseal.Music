using Abstra.Challenge.Application.Tracks;
using Abstra.Challenge.Domain;

namespace Abstra.Challenge.Application.Albums;

public static class AlbumMapper
{
    public static AlbumResponse ToResponse(this Album album) =>
        new(album.Id,
            album.Title,
            album.Artist,
            album.ReleaseDate,
            album.Tracks
                .Select((track, index)  => 
                    new AlbumTrackResponse(
                        index + 1,
                        track.Title, 
                        track.Duration.ToString(@"hh\:mm\:ss")))
                .ToList());

    public static Album ToDomain(this CreateAlbumRequest request) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Artist = request.Artist,
            ReleaseDate = request.ReleaseDate,
            Tracks = request.Tracks.Select(track => track.ToDomain()).ToList()
        };

    public static Album ToDomain(this UpdateAlbumRequest request, Guid albumId) =>
        new()
        {
            Id = albumId,
            Title = request.Title,
            Artist = request.Artist,
            ReleaseDate = request.ReleaseDate,
            Tracks = []
        };
}