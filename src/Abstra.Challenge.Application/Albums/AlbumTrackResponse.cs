using System.Diagnostics.CodeAnalysis;

namespace Abstra.Challenge.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record AlbumTrackResponse(int Number, string Title, string Duration);