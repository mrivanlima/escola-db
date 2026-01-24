namespace Escola.Application.Dtos.Content;

/// <summary>
/// DTO for resource type lookup data.
/// </summary>
public record ResourceTypeDto
{
    public Guid ResourceTypeUuid { get; init; }
    public string ResourceTypeCode { get; init; } = string.Empty;
    public string ResourceTypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? MimeTypes { get; init; }
    public int? MaxFileSizeMb { get; init; }
    public string? IconName { get; init; }
    public int DisplayOrder { get; init; }
}
