namespace Escola.Domain.Identity;

/// <summary>
/// Represents a tenant (school/organization) in the multi-tenant system.
/// </summary>
public class Tenant
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid TenantUuid { get; set; }

    /// <summary>
    /// Tenant name (e.g., "Green Valley Elementary").
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// Normalized tenant name for search (lowercase, no accents).
    /// </summary>
    public string TenantNameNormalized { get; set; } = string.Empty;

    /// <summary>
    /// Tenant type (e.g., "school", "organization").
    /// </summary>
    public string? TenantType { get; set; }

    /// <summary>
    /// JSON configuration for tenant-specific settings.
    /// </summary>
    public string? TenantConfig { get; set; }

    /// <summary>
    /// Indicates if the tenant is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Timestamp when the tenant was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this tenant.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the tenant was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this tenant.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
    public virtual ICollection<School.Student> Students { get; set; } = new List<School.Student>();
    public virtual ICollection<School.Guardian> Guardians { get; set; } = new List<School.Guardian>();
    public virtual ICollection<School.Class> Classes { get; set; } = new List<School.Class>();
    public virtual ICollection<School.Teacher> Teachers { get; set; } = new List<School.Teacher>();
    public virtual ICollection<Assets.MediaFile> MediaFiles { get; set; } = new List<Assets.MediaFile>();
}
