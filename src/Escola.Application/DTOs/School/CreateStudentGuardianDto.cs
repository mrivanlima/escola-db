namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for creating a new StudentGuardian relationship
/// </summary>
public class CreateStudentGuardianDto
{
    /// <summary>
    /// Student UUID
    /// </summary>
    public Guid StudentUuid { get; set; }

    /// <summary>
    /// Guardian UUID
    /// </summary>
    public Guid GuardianUuid { get; set; }

    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Notes about the relationship (e.g., "Lives with mother")
    /// </summary>
    public string? RelationshipNotes { get; set; }

    /// <summary>
    /// Indicates if this guardian is authorized for pickup
    /// </summary>
    public bool IsAuthorizedPickup { get; set; } = true;
}
