namespace Escola.Application.UseCases.Classes.CreateClass;

/// <summary>
/// Response after creating a class.
/// </summary>
public class CreateClassResponse
{
    public Guid ClassUuid { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string? SchoolYear { get; set; }
}
