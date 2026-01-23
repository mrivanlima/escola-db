namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for StudentGuardian response
/// </summary>
public class StudentGuardianDto
{
    /// <summary>
    /// Student UUID
    /// </summary>
    public Guid StudentUuid { get; set; }

    /// <summary>
    /// Student's full name
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Guardian UUID
    /// </summary>
    public Guid GuardianUuid { get; set; }

    /// <summary>
    /// Guardian's full name
    /// </summary>
    public string GuardianName { get; set; } = string.Empty;

    /// <summary>
    /// Relationship type name
    /// </summary>
    public string RelationshipType { get; set; } = string.Empty;

    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Notes about the relationship
    /// </summary>
    public string? RelationshipNotes { get; set; }

    /// <summary>
    /// Indicates if this guardian is authorized for pickup
    /// </summary>
    public bool IsAuthorizedPickup { get; set; }

    /// <summary>
    /// Timestamp when created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
