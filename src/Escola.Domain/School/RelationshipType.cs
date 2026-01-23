using Escola.Domain.Identity;

namespace Escola.Domain.School;

/// <summary>
/// Represents the type of relationship between a guardian and a student
/// (e.g., Father, Mother, Legal Guardian, Grandparent)
/// This is a lookup table.
/// </summary>
public class RelationshipType
{
    /// <summary>
    /// Primary key - internal identifier (SMALLINT)
    /// Maps to: relationship_type_id
    /// </summary>
    public short RelationshipTypeId { get; set; }

    /// <summary>
    /// External UUID identifier for API exposure
    /// Maps to: relationship_type_uuid
    /// </summary>
    public Guid RelationshipTypeUuid { get; set; }

    /// <summary>
    /// Foreign key to tenant
    /// Maps to: tenant_id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Relationship type name (e.g., "Father", "Mother", "Legal Guardian")
    /// Maps to: name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Normalized name for case-insensitive searches
    /// Maps to: name_normalized
    /// </summary>
    public string NameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Display order for UI sorting
    /// Maps to: display_order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this relationship type is currently active
    /// Maps to: is_active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Navigation property to tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;
}
