namespace Escola.Application.UseCases.Teachers.GetTeachers;

/// <summary>
/// Response DTO for teacher information.
/// </summary>
public class TeacherResponse
{
    public Guid TeacherUuid { get; set; }
    public string? Specialization { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? TeacherConfig { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
