namespace Escola.Application.DTOs.School;

public class CertificationDto
{
    public Guid CertificationUuid { get; set; }
    public string CertificationName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IssuingOrganization { get; set; }
    public bool IsActive { get; set; }
}
