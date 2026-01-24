namespace Escola.Application.Dtos.Content;

/// <summary>
/// DTO for activity type lookup data.
/// </summary>
public record ActivityTypeDto
{
    public Guid ActivityTypeUuid { get; init; }
    public string ActivityTypeCode { get; init; } = string.Empty;
    public string ActivityTypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? IconName { get; init; }
    public int? DefaultPoints { get; init; }
    public bool RequiresInteraction { get; init; }
    public int DisplayOrder { get; init; }
}
