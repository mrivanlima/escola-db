using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class ProficiencyLevel
{
    public short ProficiencyLevelId { get; set; }
    public Guid ProficiencyUuid { get; set; }
    public int TenantId { get; set; }
    
    public string ProficiencyCode { get; set; } = string.Empty;
    public string ProficiencyCodeNormalized { get; set; } = string.Empty;
    public string ProficiencyName { get; set; } = string.Empty;
    public string ProficiencyNameNormalized { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MinimumYears { get; set; }
    public string? IconName { get; set; }
    public string? ColorCode { get; set; }
    public short DisplayOrder { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
