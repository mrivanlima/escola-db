namespace Escola.Application.DTOs.School;

public class CreateTeacherDto
{
    public Guid TenantUuid { get; set; }
    public Guid UserUuid { get; set; }
    public Guid? SpecializationUuid { get; set; }
    public DateOnly? HireDate { get; set; }
    public bool IsActive { get; set; } = true;
}
