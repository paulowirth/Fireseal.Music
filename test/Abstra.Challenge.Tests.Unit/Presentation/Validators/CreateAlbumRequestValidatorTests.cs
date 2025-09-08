using Abstra.Challenge.Application.Albums;
using Abstra.Challenge.Presentation.Albums;
using FluentValidation.TestHelper;

namespace Abstra.Challenge.Tests.Unit.Presentation.Validators;

public sealed class CreateAlbumRequestValidatorTests
{
    private readonly CreateAlbumRequestValidator _validator = new();

    private static CreateAlbumTrackRequest ValidTrack() => 
        new("Track Title", "00:03:30", "USRC17607839");

    [Fact]
    public void Validate_ShouldPass_WhenRequestIsValid()
    {
        //Arrange
        var request = 
            new CreateAlbumRequest(
                "Album Title",
                "Artist Name",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                new List<CreateAlbumTrackRequest> { ValidTrack() });

        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldFail_WhenTitleIsNullOrEmpty(string? title)
    {
        //Arrange
        var request = 
            new CreateAlbumRequest(
                title!,
                "Artist Name",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                new List<CreateAlbumTrackRequest> { ValidTrack() });

        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result
            .ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(AlbumResources.TitleIsRequired);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleExceedsMaxLength()
    {
        //Arrange
        var saveRequest = 
            new CreateAlbumRequest(
                new string('A', 51),
                "Artist Name",
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                new List<CreateAlbumTrackRequest> { ValidTrack() });

        //Act
        var result = _validator.TestValidate(saveRequest);
        
        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Title)
            .WithErrorMessage(AlbumResources.TitleMaxLengthExceeded);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldFail_WhenArtistIsNullOrEmpty(string? artist)
    {
        //Arrange
        var saveRequest = 
            new CreateAlbumRequest(
                "Album Title",
                artist!,
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                new List<CreateAlbumTrackRequest> { ValidTrack() });

        //Act
        var result = _validator.TestValidate(saveRequest);
        
        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Artist)
            .WithErrorMessage(AlbumResources.ArtistIsRequired);
    }

    [Fact]
    public void Validate_ShouldFail_WhenArtistExceedsMaxLength()
    {
        //Arrange
        var saveRequest = 
            new CreateAlbumRequest(
                "Album Title",
                new string('A', 51),
                DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime),
                new List<CreateAlbumTrackRequest> { ValidTrack() }
        );

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Artist)
            .WithErrorMessage(AlbumResources.ArtistMaxLengthExceeded);
    }

    [Fact]
    public void Validate_ShouldFail_WhenReleaseDateIsInFuture()
    {
        //Arrange
        var futureDate = DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime.AddDays(1));
        var saveRequest = 
            new CreateAlbumRequest("Album Title",
            "Artist Name",
            futureDate,
            new List<CreateAlbumTrackRequest> { ValidTrack() });

        //Act
        var result = _validator.TestValidate(saveRequest);
        
        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.ReleaseDate)
            .WithErrorMessage(AlbumResources.ReleaseDateCannotBeGreaterThanTodaysDate);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTracksIsNull()
    {
        //Arrange
        var request = 
            new CreateAlbumRequest(
                "Album Title",
                "Artist Name",
                DateOnly.FromDateTime(DateTime.UtcNow),
                null!);

        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Tracks)
            .WithErrorMessage(AlbumResources.AtLeastOneTrackIsRequired);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTracksIsEmpty()
    {
        //Arrange
        var saveRequest = 
            new CreateAlbumRequest(
                "Album Title",
                "Artist Name",
                DateOnly.FromDateTime(DateTime.UtcNow),
                new List<CreateAlbumTrackRequest>()
            );

        //Act
        var result = _validator.TestValidate(saveRequest);
        
        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Tracks)
            .WithErrorMessage(AlbumResources.AtLeastOneTrackIsRequired);
    }

    [Fact]
    public void Validate_ShouldFail_WhenAnyTrackIsInvalid()
    {
        //Arrange
        var invalidTrack = 
            new CreateAlbumTrackRequest(string.Empty, "00:00:00", string.Empty);
        
        var request = 
            new CreateAlbumRequest(
                "Album Title",
                "Artist Name",
                DateOnly.FromDateTime(DateTime.UtcNow),
                new List<CreateAlbumTrackRequest> { ValidTrack(), invalidTrack }
            );

        //Act
        var result = _validator.TestValidate(request);
        
        //Assert
        result.ShouldHaveValidationErrorFor("Tracks[1].Title");
        result.ShouldHaveValidationErrorFor("Tracks[1].Duration");
        result.ShouldHaveValidationErrorFor("Tracks[1].Isrc");
    }
}
