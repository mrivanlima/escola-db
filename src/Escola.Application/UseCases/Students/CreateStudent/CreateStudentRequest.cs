namespace Escola.Application.UseCases.Students.CreateStudent;

/// <summary>
/// Request for creating a new student.
/// </summary>
public class CreateStudentRequest
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
    /// Student's date of birth.
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Tenant ID (school/organization).
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Optional nickname.
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    /// Optional middle name.
    /// </summary>
    public string? MiddleName { get; set; }
}
