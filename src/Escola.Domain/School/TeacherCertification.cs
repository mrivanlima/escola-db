using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class TeacherCertification
{
    public int TeacherId { get; set; }
    public int CertificationId { get; set; }
    
    public DateOnly? ObtainedDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? CredentialNumber { get; set; }
    public string? Notes { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public Teacher Teacher { get; set; } = null!;
    public Certification Certification { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
