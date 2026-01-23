namespace Escola.Application.UseCases.Students.CreateStudent;

/// <summary>
/// Response after creating a student.
/// </summary>
public class CreateStudentResponse
{
    /// <summary>
    /// The newly created student's UUID.
    /// </summary>
    public Guid StudentUuid { get; set; }

    /// <summary>
    /// Full name of the created student.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
