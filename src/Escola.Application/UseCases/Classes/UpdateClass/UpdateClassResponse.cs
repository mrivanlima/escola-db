namespace Escola.Application.UseCases.Classes.UpdateClass;

/// <summary>
/// Response for updating a class.
/// </summary>
public class UpdateClassResponse
{
    public Guid ClassUuid { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string? SchoolYear { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
