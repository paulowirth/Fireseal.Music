using System.Diagnostics.CodeAnalysis;

namespace Abstra.Challenge.Application.Tracks;

[ExcludeFromCodeCoverage]
public sealed record TrackResponse(
    Guid Id,
    string Title, 
    string Album, 
    string Artist, 
    string Duration, 
    string Isrc);