using Fireseal.Music.Application.Tracks;
using FluentValidation;

namespace Fireseal.Music.Presentation.Tracks;

public sealed class SaveTrackRequestValidator : AbstractValidator<SaveTrackRequest>
{
    public SaveTrackRequestValidator()
    {
        RuleFor(request => request.AlbumId)
            .NotEmpty()
            .WithMessage(TrackResources.AlbumIdIsRequired);
        
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