namespace Escola.Application.Assets.MediaFiles;

public record CreateMediaFileDto
{
    public required Guid MimeTypeUuid { get; init; }
    public required string OriginalName { get; init; }
    public required string StoragePath { get; init; }
    public long? SizeBytes { get; init; }
    public string? AltText { get; init; }
    public string? Metadata { get; init; }
}
