using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class Teacher
{
    public int TeacherId { get; set; }
    public Guid TeacherUuid { get; set; }
    public int TenantId { get; set; }
    public int UserId { get; set; }
    public int? SpecializationId { get; set; }
    public DateOnly? HireDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public Specialization? Specialization { get; set; }
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
