namespace Abstra.Challenge.Presentation.Tracks;

public static class TrackResources
{
    public const string AlbumIdIsRequired = "A valid AlbumId is required";
    public const string TitleIsRequired = "Title is required";
    public const string TitleMaxLengthExceeded = "Title should not exceed 50 characters";
    public const string DurationIsRequired = "Duration is required";
    public const string DurationMustBePositive = "Duration must be a positive number.";
    public const string IsrcIsRequired = "ISRC is required";
    public const string IsrcRegex = "^[A-Z]{2}[A-Z0-9]{3}[0-9]{7}$";
    public const string IsrcMustMatchTheRequiredPattern = "IRSC must be 12 characters, alphanumeric (A-Z, 0-9)";
}