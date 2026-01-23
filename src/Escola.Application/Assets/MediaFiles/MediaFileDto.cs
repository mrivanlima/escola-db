namespace Escola.Application.Assets.MediaFiles;

public record MediaFileDto
{
    public required Guid FileUuid { get; init; }
    public required Guid TenantUuid { get; init; }
    public required string TenantName { get; init; }
    public required Guid MimeTypeUuid { get; init; }
    public required string MimeTypeCode { get; init; }
    public required string OriginalName { get; init; }
    public required string StoragePath { get; init; }
    public long? SizeBytes { get; init; }
    public string? AltText { get; init; }
    public string? Metadata { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
