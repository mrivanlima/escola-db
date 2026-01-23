namespace Escola.Application.DTOs.School;

public class CreateClassStudentDto
{
    public Guid ClassUuid { get; set; }
    public Guid StudentUuid { get; set; }
    public DateOnly EnrollmentDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public Guid StatusUuid { get; set; }
}
