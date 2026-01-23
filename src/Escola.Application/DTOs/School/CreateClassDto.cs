namespace Escola.Application.DTOs.School;

public class CreateClassDto
{
    public Guid TenantUuid { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public Guid? GradeLevelUuid { get; set; }
    public Guid SchoolYearUuid { get; set; }
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
    public bool IsActive { get; set; } = true;
}
