namespace Escola.Application.UseCases.Students.GetStudents;

/// <summary>
/// Response DTO for student information.
/// </summary>
public class StudentResponse
{
    public Guid StudentUuid { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Nickname { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? AvatarConfig { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
