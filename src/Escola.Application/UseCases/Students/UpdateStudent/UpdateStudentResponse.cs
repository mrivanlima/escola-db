using Escola.Application.UseCases.Students.GetStudents;

namespace Escola.Application.UseCases.Students.UpdateStudent;

/// <summary>
/// Response for updating a student.
/// </summary>
public class UpdateStudentResponse
{
    public Guid StudentUuid { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Nickname { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
