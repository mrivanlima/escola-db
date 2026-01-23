namespace Escola.Application.DTOs.School;

public class UpdateClassDto
{
    public string? ClassName { get; set; }
    public Guid? GradeLevelUuid { get; set; }
    public Guid? SchoolYearUuid { get; set; }
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
    public bool? IsActive { get; set; }
}
