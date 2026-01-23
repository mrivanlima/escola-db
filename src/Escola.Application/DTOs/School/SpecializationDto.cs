namespace Escola.Application.DTOs.School;

public class SpecializationDto
{
    public Guid SpecializationUuid { get; set; }
    public string SpecializationName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
