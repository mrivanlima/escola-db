namespace Escola.Application.DTOs.School;

public class SubjectDto
{
    public Guid SubjectUuid { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? GradeLevelUuid { get; set; }
    public string? GradeLevelName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateSubjectDto
{
    public string SubjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? GradeLevelUuid { get; set; }
}

public class UpdateSubjectDto
{
    public string SubjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? GradeLevelUuid { get; set; }
    public bool IsActive { get; set; }
}
