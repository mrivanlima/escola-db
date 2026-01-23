namespace Escola.Application.DTOs.School;

public class TeacherCertificationDto
{
    public Guid TeacherUuid { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public Guid CertificationUuid { get; set; }
    public string CertificationName { get; set; } = string.Empty;
    public DateOnly? ObtainedDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? CredentialNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTeacherCertificationDto
{
    public Guid TeacherUuid { get; set; }
    public Guid CertificationUuid { get; set; }
    public DateOnly? ObtainedDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? CredentialNumber { get; set; }
    public string? Notes { get; set; }
}

public class UpdateTeacherCertificationDto
{
    public DateOnly? ObtainedDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? CredentialNumber { get; set; }
    public string? Notes { get; set; }
}
