namespace Escola.Application.DTOs.Game;

/// <summary>
/// DTO for progress status lookup data.
/// </summary>
public record ProgressStatusDto
{
    public Guid StatusUuid { get; init; }
    public string StatusCode { get; init; } = string.Empty;
    public string StatusName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? IconName { get; init; }
    public string? ColorCode { get; init; }
    public bool IsFinalState { get; init; }
    public int DisplayOrder { get; init; }
}
