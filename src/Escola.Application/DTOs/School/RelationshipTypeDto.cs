namespace Escola.Application.DTOs.School;

/// <summary>
/// Data Transfer Object for RelationshipType (read-only lookup table)
/// </summary>
public class RelationshipTypeDto
{
    /// <summary>
    /// External UUID identifier
    /// </summary>
    public Guid RelationshipTypeUuid { get; set; }

    /// <summary>
    /// Tenant UUID
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Relationship type name (e.g., "Father", "Mother", "Legal Guardian")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display order for UI sorting
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this relationship type is currently active
    /// </summary>
    public bool IsActive { get; set; }
}
