using Fireseal.Music.Application.Albums;
using Fireseal.Music.Presentation.Tracks;
using FluentValidation;

namespace Fireseal.Music.Presentation.Albums;

public class CreateAlbumTrackRequestValidator : AbstractValidator<CreateAlbumTrackRequest>
{
    public CreateAlbumTrackRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage(TrackResources.TitleIsRequired)
            .MaximumLength(50)
            .WithMessage(TrackResources.TitleMaxLengthExceeded);

        RuleFor(request => request.Duration)
            .NotEmpty()
            .WithMessage(TrackResources.DurationIsRequired)
            .Must(duration => 
                TimeSpan.TryParse(duration, out var parsedDuration) && parsedDuration > TimeSpan.Zero)
            .WithMessage(TrackResources.DurationMustBePositive);

        RuleFor(request => request.Isrc)
            .NotEmpty()
            .WithMessage(TrackResources.IsrcIsRequired)
            .Matches(TrackResources.IsrcRegex)
            .WithMessage(TrackResources.IsrcMustMatchTheRequiredPattern);
    }
}