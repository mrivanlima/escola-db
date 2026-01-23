namespace Escola.Application.UseCases.Teachers.UpdateTeacher;

/// <summary>
/// Request for updating a teacher.
/// </summary>
public class UpdateTeacherRequest
{
    public string? Specialization { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? TeacherConfig { get; set; }
}
