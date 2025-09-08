using System.Diagnostics.CodeAnalysis;

namespace Abstra.Challenge.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record CreateAlbumTrackRequest(string Title, string Duration, string Isrc);