using Abstra.Challenge.Application.Albums;
using FluentValidation;

namespace Abstra.Challenge.Presentation.Albums;

public sealed class CreateAlbumRequestValidator : SaveAlbumRequestValidator<CreateAlbumRequest>
{
    public CreateAlbumRequestValidator()
    {
        RuleFor(request => request.Tracks)
            .NotEmpty()
            .WithMessage(AlbumResources.AtLeastOneTrackIsRequired);

        RuleForEach(request => request.Tracks)
            .SetValidator(new CreateAlbumTrackRequestValidator());
    }
}