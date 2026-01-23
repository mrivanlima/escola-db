namespace Escola.Application.UseCases.Classes.CreateClass;

/// <summary>
/// Request for creating a new class.
/// </summary>
public class CreateClassRequest
{
    public int TenantId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string? GradeLevel { get; set; }
    public string SchoolYear { get; set; } = string.Empty;
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
}
