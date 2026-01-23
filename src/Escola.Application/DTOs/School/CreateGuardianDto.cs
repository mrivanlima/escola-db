namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for creating a new Guardian
/// </summary>
public class CreateGuardianDto
{
    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Associated user UUID (links to AppUser)
    /// Must be an existing user with 'Guardian' or 'Parent' role
    /// </summary>
    public Guid UserUuid { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Relationship type UUID (e.g., Father, Mother, Legal Guardian)
    /// </summary>
    public Guid RelationshipTypeUuid { get; set; }

    /// <summary>
    /// Indicates if this is the primary guardian
    /// </summary>
    public bool IsPrimary { get; set; } = false;
}
