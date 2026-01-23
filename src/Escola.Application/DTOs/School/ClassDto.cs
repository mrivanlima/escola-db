namespace Escola.Application.DTOs.School;

public class ClassDto
{
    public Guid ClassUuid { get; set; }
    public Guid TenantUuid { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public Guid? GradeLevelUuid { get; set; }
    public string? GradeLevelName { get; set; }
    public Guid SchoolYearUuid { get; set; }
    public string SchoolYearName { get; set; } = string.Empty;
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
