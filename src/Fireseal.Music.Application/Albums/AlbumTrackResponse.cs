using System.Diagnostics.CodeAnalysis;

namespace Fireseal.Music.Application.Albums;

[ExcludeFromCodeCoverage]
public sealed record AlbumTrackResponse(int Number, string Title, string Duration);