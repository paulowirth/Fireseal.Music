using Fireseal.Music.Application.Tracks;
using Fireseal.Music.Presentation.Tracks;
using FluentValidation.TestHelper;

namespace Fireseal.Music.Tests.Unit.Presentation.Validators;

public sealed class SaveTrackRequestValidatorTests
{
    private readonly SaveTrackRequestValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenRequestIsValid()
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(),"Track Title", "00:03:30", "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ShouldFail_WhenAlbumIdEmpty()
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(Guid.Empty, "Track Title", "00:03:30", "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.AlbumId)
            .WithErrorMessage(TrackResources.AlbumIdIsRequired);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldFail_WhenTitleIsNullOrEmpty(string? title)
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(), title!, "00:03:30", "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Title)
            .WithErrorMessage(TrackResources.TitleIsRequired);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleExceedsMaxLength()
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(),new string('A', 51), "00:03:30", "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Title)
            .WithErrorMessage(TrackResources.TitleMaxLengthExceeded);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldFail_WhenDurationIsNullOrEmpty(string? duration)
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
            Guid.CreateVersion7(), "Track Title", duration!, "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Duration)
            .WithErrorMessage(TrackResources.DurationIsRequired);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("abc")]
    [InlineData("99:99:99")]
    [InlineData("00:00:00")]
    [InlineData("-00:01:00")]
    public void Validate_ShouldFail_WhenDurationIsInvalid(string duration)
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(),"Track Title", duration, "USRC17607839");

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Duration)
            .WithErrorMessage(TrackResources.DurationMustBePositive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldFail_WhenIsrcIsNullOrEmpty(string? isrc)
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(),"Track Title", "00:03:30", isrc!);

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Isrc)
            .WithErrorMessage(TrackResources.IsrcIsRequired);
    }

    [Theory]
    [InlineData("INVALID!")]
    [InlineData("123")]
    [InlineData("USRC1760783!")]
    [InlineData("USRC17607839123")]
    public void Validate_ShouldFail_WhenIsrcDoesNotMatchPattern(string isrc)
    {
        //Arrange
        var saveRequest = 
            new SaveTrackRequest(
                Guid.CreateVersion7(),"Track Title", "00:03:30", isrc);

        //Act
        var result = _validator.TestValidate(saveRequest);

        //Assert
        result
            .ShouldHaveValidationErrorFor(request => request.Isrc)
            .WithErrorMessage(TrackResources.IsrcMustMatchTheRequiredPattern);
    }
}