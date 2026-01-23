using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class ClassStudent
{
    public int ClassStudentId { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public int TenantId { get; set; }
    public DateOnly EnrollmentDate { get; set; }
    public short StatusId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public Class Class { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public EnrollmentStatus Status { get; set; } = null!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
