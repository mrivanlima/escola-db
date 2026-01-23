namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for Guardian response
/// </summary>
public class GuardianDto
{
    /// <summary>
    /// External UUID identifier
    /// </summary>
    public Guid GuardianUuid { get; set; }

    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Associated user UUID (links to AppUser)
    /// </summary>
    public Guid UserUuid { get; set; }

    /// <summary>
    /// User's full name
    /// </summary>
    public string UserFullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email
    /// </summary>
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Relationship type UUID
    /// </summary>
    public Guid RelationshipTypeUuid { get; set; }

    /// <summary>
    /// Relationship type name
    /// </summary>
    public string RelationshipTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is the primary guardian
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Timestamp when created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UUID of user who created this guardian
    /// </summary>
    public Guid? CreatedByUuid { get; set; }

    /// <summary>
    /// Timestamp when last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// UUID of user who last updated this guardian
    /// </summary>
    public Guid? UpdatedByUuid { get; set; }
}
