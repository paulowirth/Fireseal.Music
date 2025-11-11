using System.Diagnostics.CodeAnalysis;
using Fireseal.Music.Application.Albums;

namespace Fireseal.Music.Presentation.Albums;

[ExcludeFromCodeCoverage]
public sealed class UpdateAlbumRequestValidator : SaveAlbumRequestValidator<UpdateAlbumRequest>;