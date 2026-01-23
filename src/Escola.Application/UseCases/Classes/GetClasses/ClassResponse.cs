namespace Escola.Application.UseCases.Classes.GetClasses;

/// <summary>
/// Response DTO for class information.
/// </summary>
public class ClassResponse
{
    public Guid ClassUuid { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string? GradeLevel { get; set; }
    public string? SchoolYear { get; set; }
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
