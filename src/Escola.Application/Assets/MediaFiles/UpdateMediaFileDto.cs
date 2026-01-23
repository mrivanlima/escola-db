namespace Escola.Application.Assets.MediaFiles;

public record UpdateMediaFileDto
{
    public string? AltText { get; init; }
    public string? Metadata { get; init; }
}
