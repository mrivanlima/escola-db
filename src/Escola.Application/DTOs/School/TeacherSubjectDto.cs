namespace Escola.Application.DTOs.School;

public class TeacherSubjectDto
{
    public Guid TeacherUuid { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public Guid SubjectUuid { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public Guid? ProficiencyUuid { get; set; }
    public string? ProficiencyName { get; set; }
    public int? YearsExperience { get; set; }
    public bool IsPrimarySubject { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTeacherSubjectDto
{
    public Guid TeacherUuid { get; set; }
    public Guid SubjectUuid { get; set; }
    public Guid? ProficiencyUuid { get; set; }
    public int? YearsExperience { get; set; }
    public bool IsPrimarySubject { get; set; }
    public string? Notes { get; set; }
}

public class UpdateTeacherSubjectDto
{
    public Guid? ProficiencyUuid { get; set; }
    public int? YearsExperience { get; set; }
    public bool IsPrimarySubject { get; set; }
    public string? Notes { get; set; }
}
