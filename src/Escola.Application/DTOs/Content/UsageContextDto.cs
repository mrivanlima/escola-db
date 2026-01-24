namespace Escola.Application.Dtos.Content;

/// <summary>
/// DTO for usage context lookup data.
/// </summary>
public record UsageContextDto
{
    public Guid UsageContextUuid { get; init; }
    public string ContextCode { get; init; } = string.Empty;
    public string ContextName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? IconName { get; init; }
    public int DisplayOrder { get; init; }
}
