using System.Diagnostics.CodeAnalysis;

namespace Fireseal.Music.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record CreateAlbumTrackRequest(string Title, string Duration, string Isrc);