namespace Escola.Application.DTOs.School;

public class UpdateClassStudentDto
{
    public DateOnly? EnrollmentDate { get; set; }
    public Guid? StatusUuid { get; set; }
}
