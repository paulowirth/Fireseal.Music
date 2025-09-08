using System.Diagnostics.CodeAnalysis;
using Abstra.Challenge.Application.Albums;

namespace Abstra.Challenge.Presentation.Albums;

[ExcludeFromCodeCoverage]
public sealed class UpdateAlbumRequestValidator : SaveAlbumRequestValidator<UpdateAlbumRequest>;