namespace Escola.Application.DTOs.School;

public class TeacherDto
{
    public Guid TeacherUuid { get; set; }
    public Guid TenantUuid { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public Guid UserUuid { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public Guid? SpecializationUuid { get; set; }
    public string? SpecializationName { get; set; }
    public DateOnly? HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
