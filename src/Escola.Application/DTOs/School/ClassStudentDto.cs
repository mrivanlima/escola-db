namespace Escola.Application.DTOs.School;

public class ClassStudentDto
{
    public Guid ClassUuid { get; set; }
    public Guid StudentUuid { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateOnly EnrollmentDate { get; set; }
    public Guid StatusUuid { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
