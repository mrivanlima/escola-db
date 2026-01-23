using Escola.Domain.Identity;

namespace Escola.Domain.School;

/// <summary>
/// Many-to-many relationship between students and guardians.
/// </summary>
public class StudentGuardian
{
    /// <summary>
    /// Internal database ID.
    /// Maps to: student_guardian_id
    /// </summary>
    public int StudentGuardianId { get; set; }

    /// <summary>
    /// Student ID.
    /// Maps to: student_id
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Guardian ID.
    /// Maps to: guardian_id
    /// </summary>
    public int GuardianId { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// Maps to: tenant_id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Notes about the relationship.
    /// Maps to: relationship_notes
    /// </summary>
    public string? RelationshipNotes { get; set; }

    /// <summary>
    /// Normalized relationship notes for search (lowercase, no accents).
    /// Maps to: relationship_notes_normalized
    /// </summary>
    public string? RelationshipNotesNormalized { get; set; }

    /// <summary>
    /// Indicates if this guardian is authorized for pickup.
    /// Maps to: is_authorized_pickup
    /// </summary>
    public bool IsAuthorizedPickup { get; set; }

    /// <summary>
    /// Timestamp when the relationship was created.
    /// Maps to: created_at
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this relationship.
    /// Maps to: created_by
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the relationship was last updated.
    /// Maps to: updated_at
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this relationship.
    /// Maps to: updated_by
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// Maps to: deleted_at
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Navigation property to student
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Navigation property to guardian
    /// </summary>
    public Guardian Guardian { get; set; } = null!;

    /// <summary>
    /// Navigation property to tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Navigation property to created by user
    /// </summary>
    public AppUser? CreatedByUser { get; set; }

    /// <summary>
    /// Navigation property to updated by user
    /// </summary>
    public AppUser? UpdatedByUser { get; set; }
}
