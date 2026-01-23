namespace Escola.Domain.School;

/// <summary>
/// Many-to-many relationship between students and guardians.
/// </summary>
public class StudentGuardian
{
    /// <summary>
    /// Internal database ID.
    /// </summary>
    public int StudentGuardianId { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Guardian ID.
    /// </summary>
    public int GuardianId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Notes about the relationship.
    /// </summary>
    public string? RelationshipNotes { get; set; }

    /// <summary>
    /// Normalized relationship notes for search (lowercase, no accents).
    /// </summary>
    public string? RelationshipNotesNormalized { get; set; }

    /// <summary>
    /// Indicates if this guardian is authorized for pickup.
    /// </summary>
    public bool IsAuthorizedPickup { get; set; }

    /// <summary>
    /// Timestamp when the relationship was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this relationship.
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the relationship was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this relationship.
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    // Navigation properties
    public virtual Student Student { get; set; } = null!;
    public virtual Guardian Guardian { get; set; } = null!;
    public virtual Identity.Tenant Tenant { get; set; } = null!;
}
