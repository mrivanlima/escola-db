namespace Escola.Application.UseCases.Students.UpdateStudent;

/// <summary>
/// Request for updating a student.
/// </summary>
public class UpdateStudentRequest
{
    /// <summary>
    /// Student's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Student's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Student's middle name.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Student's nickname.
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// Student's date of birth.
    /// </summary>
    public DateOnly BirthDate { get; set; }
}
