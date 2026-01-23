using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class Certification
{
    public int CertificationId { get; set; }
    public Guid CertificationUuid { get; set; }
    
    public string CertificationName { get; set; } = string.Empty;
    public string CertificationNameNormalized { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IssuingOrganization { get; set; }
    public bool IsActive { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
