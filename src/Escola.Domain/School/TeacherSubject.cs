using Escola.Domain.Identity;

namespace Escola.Domain.School;

public class TeacherSubject
{
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    
    public short? ProficiencyLevelId { get; set; }
    public int? YearsExperience { get; set; }
    public bool IsPrimarySubject { get; set; }
    public string? Notes { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    // Navigation properties
    public Teacher Teacher { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public ProficiencyLevel? ProficiencyLevel { get; set; }
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
