namespace Escola.Application.UseCases.Teachers.CreateTeacher;

/// <summary>
/// Response after creating a teacher.
/// </summary>
public class CreateTeacherResponse
{
    public Guid TeacherUuid { get; set; }
    public string? Specialization { get; set; }
}
