namespace Escola.Application.UseCases.Classes.UpdateClass;

/// <summary>
/// Request for updating a class.
/// </summary>
public class UpdateClassRequest
{
    public string? ClassName { get; set; }
    public string? GradeLevel { get; set; }
    public string? SchoolYear { get; set; }
    public int? MaxStudents { get; set; }
    public string? ClassConfig { get; set; }
}
