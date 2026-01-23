namespace Escola.Application.UseCases.Teachers.CreateTeacher;

/// <summary>
/// Request for creating a new teacher.
/// </summary>
public class CreateTeacherRequest
{
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public string? Specialization { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? TeacherConfig { get; set; }
}
