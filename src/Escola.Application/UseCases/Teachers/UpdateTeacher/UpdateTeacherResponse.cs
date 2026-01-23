namespace Escola.Application.UseCases.Teachers.UpdateTeacher;

/// <summary>
/// Response for updating a teacher.
/// </summary>
public class UpdateTeacherResponse
{
    public Guid TeacherUuid { get; set; }
    public string? Specialization { get; set; }
    public DateOnly? HireDate { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
