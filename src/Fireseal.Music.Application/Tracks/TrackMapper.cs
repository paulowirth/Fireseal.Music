using Fireseal.Music.Domain;
using Fireseal.Music.Application.Albums;

namespace Fireseal.Music.Application.Tracks;

public static class TrackMapper
{
    public static TrackResponse ToResponse(this Track track) => 
        new(track.Id,
            track.Title,
            track.Album!.Title,
            track.Album!.Artist,
            track.Duration.ToString(@"hh\:mm\:ss"),
            track.Isrc);

    public static Track ToDomain(this CreateAlbumTrackRequest createRequest) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Title = createRequest.Title,
            Duration = TimeSpan.Parse(createRequest.Duration),
            Isrc = createRequest.Isrc
        };

    public static Track ToDomain(this SaveTrackRequest createRequest) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            AlbumId = createRequest.AlbumId,
            Title = createRequest.Title,
            Duration = TimeSpan.Parse(createRequest.Duration),
            Isrc = createRequest.Isrc
        };

    public static Track ToDomain(this SaveTrackRequest updateRequest, Guid trackId) =>
        new()
        {
            Id = trackId,
            AlbumId = updateRequest.AlbumId,
            Title = updateRequest.Title,
            Duration = TimeSpan.Parse(updateRequest.Duration),
            Isrc = updateRequest.Isrc
        };
}