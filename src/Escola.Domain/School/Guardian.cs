namespace Escola.Domain.School;

/// <summary>
/// Represents a guardian (parent/legal guardian).
/// </summary>
public class Guardian
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int GuardianId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// </summary>
    public Guid GuardianUuid { get; set; }

    /// <summary>
    /// Tenant ID this guardian belongs to.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Associated user ID (links to AppUser).
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Relationship type (e.g., "mother", "father", "legal_guardian").
    /// </summary>
    public string? Relationship { get; set; }

    /// <summary>
    /// Indicates if this is the primary guardian.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Timestamp when the guardian was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this guardian.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the guardian was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this guardian.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Identity.Tenant Tenant { get; set; } = null!;
    public virtual Identity.AppUser User { get; set; } = null!;
    public virtual ICollection<StudentGuardian> StudentGuardians { get; set; } = new List<StudentGuardian>();
}
