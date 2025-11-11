using Fireseal.Music.Application.Albums;
using FluentValidation;

namespace Fireseal.Music.Presentation.Albums;

public abstract class SaveAlbumRequestValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : SaveAlbumRequest
{
    protected SaveAlbumRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage(AlbumResources.TitleIsRequired)
            .MaximumLength(50)
            .WithMessage(AlbumResources.TitleMaxLengthExceeded);
        
        RuleFor(request => request.Artist)
            .NotEmpty()
            .WithMessage(AlbumResources.ArtistIsRequired)
            .MaximumLength(50)
            .WithMessage(AlbumResources.ArtistMaxLengthExceeded);
        
        RuleFor(request => request.ReleaseDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime))
            .WithMessage(AlbumResources.ReleaseDateCannotBeGreaterThanTodaysDate);
    }
}