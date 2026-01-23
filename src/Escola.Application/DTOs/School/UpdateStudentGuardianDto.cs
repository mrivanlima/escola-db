namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for updating an existing StudentGuardian relationship
/// </summary>
public class UpdateStudentGuardianDto
{
    /// <summary>
    /// Notes about the relationship
    /// </summary>
    public string? RelationshipNotes { get; set; }

    /// <summary>
    /// Indicates if this guardian is authorized for pickup
    /// </summary>
    public bool? IsAuthorizedPickup { get; set; }
}
