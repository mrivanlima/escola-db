namespace Escola.Application.DTOs.School;

public class ProficiencyLevelDto
{
    public Guid ProficiencyUuid { get; set; }
    public Guid TenantUuid { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string ProficiencyCode { get; set; } = string.Empty;
    public string ProficiencyName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MinimumYears { get; set; }
    public string? IconName { get; set; }
    public string? ColorCode { get; set; }
    public short DisplayOrder { get; set; }
}
