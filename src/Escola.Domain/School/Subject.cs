using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class Subject
{
    public int SubjectId { get; set; }
    public Guid SubjectUuid { get; set; }
    
    public string SubjectName { get; set; } = string.Empty;
    public string SubjectNameNormalized { get; set; } = string.Empty;
    public string? Description { get; set; }
    public short? GradeLevelId { get; set; }
    public bool IsActive { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public GradeLevel? GradeLevel { get; set; }
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
