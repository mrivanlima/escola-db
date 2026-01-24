namespace Escola.Application.Dtos.Content;

/// <summary>
/// DTO for module type lookup data.
/// </summary>
public record ModuleTypeDto
{
    public Guid ModuleTypeUuid { get; init; }
    public string ModuleTypeCode { get; init; } = string.Empty;
    public string ModuleTypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? IconName { get; init; }
    public string? ColorCode { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsActive { get; init; }
}
