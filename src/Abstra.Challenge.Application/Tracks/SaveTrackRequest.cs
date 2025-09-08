namespace Abstra.Challenge.Application.Tracks;

public record SaveTrackRequest(Guid AlbumId, string Title, string Duration, string Isrc);