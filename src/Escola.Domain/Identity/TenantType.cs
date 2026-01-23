namespace Escola.Domain.Identity;

/// <summary>
/// Represents a type/category of tenant (e.g., School, Organization, Individual).
/// Lookup table for tenant classification.
/// </summary>
public class TenantType
{
    /// <summary>
    /// Internal primary key. Never exposed to API.
    /// </summary>
    public short TenantTypeId { get; set; }

    /// <summary>
    /// External UUID for API exposure. This is the only ID exposed to clients.
    /// </summary>
    public Guid TenantTypeUuid { get; set; }

    /// <summary>
    /// Display name of the tenant type (e.g., "School", "Organization", "Individual").
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized version of TypeName (lowercase, unaccented) for searching.
    /// </summary>
    public string TypeNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of this tenant type.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this tenant type is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual AppUser? Creator { get; set; }
    public virtual AppUser? Updater { get; set; }
}
