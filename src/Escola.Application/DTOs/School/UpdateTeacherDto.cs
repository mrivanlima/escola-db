namespace Escola.Application.DTOs.School;

public class UpdateTeacherDto
{
    public Guid? SpecializationUuid { get; set; }
    public DateOnly? HireDate { get; set; }
    public bool? IsActive { get; set; }
}
