using Escola.Domain.Identity;

namespace Escola.Domain.School;

/// <summary>
/// Represents a guardian (parent/legal guardian).
/// </summary>
public class Guardian
{
    /// <summary>
    /// Internal database ID.
    /// Maps to: guardian_id
    /// </summary>
    public int GuardianId { get; set; }

    /// <summary>
    /// External UUID for API exposure.
    /// Maps to: guardian_uuid
    /// </summary>
    public Guid GuardianUuid { get; set; }

    /// <summary>
    /// Tenant ID this guardian belongs to.
    /// Maps to: tenant_id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Associated user ID (links to AppUser).
    /// Maps to: user_id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Phone number.
    /// Maps to: phone_number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Foreign key to relationship type.
    /// Maps to: relationship_type_id
    /// </summary>
    public short RelationshipTypeId { get; set; }

    /// <summary>
    /// Indicates if this is the primary guardian.
    /// Maps to: is_primary
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Timestamp when the guardian was created.
    /// Maps to: created_at
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// User ID who created this guardian.
    /// Maps to: created_by
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Timestamp when the guardian was last updated.
    /// Maps to: updated_at
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// User ID who last updated this guardian.
    /// Maps to: updated_by
    /// </summary>
    public int? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete timestamp.
    /// Maps to: deleted_at
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Navigation property to tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Navigation property to app user
    /// </summary>
    public AppUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property to relationship type
    /// </summary>
    public RelationshipType RelationshipType { get; set; } = null!;

    /// <summary>
    /// Navigation property to created by user
    /// </summary>
    public AppUser? CreatedByUser { get; set; }

    /// <summary>
    /// Navigation property to updated by user
    /// </summary>
    public AppUser? UpdatedByUser { get; set; }

    /// <summary>
    /// Navigation property to student guardians
    /// </summary>
    public ICollection<StudentGuardian> StudentGuardians { get; set; } = new List<StudentGuardian>();
}
