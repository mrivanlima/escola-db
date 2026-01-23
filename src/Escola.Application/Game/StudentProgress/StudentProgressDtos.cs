namespace Escola.Application.Game.StudentProgress;

public record StudentProgressDto
{
    public required Guid ProgressUuid { get; init; }
    public required Guid StudentUuid { get; init; }
    public required string StudentName { get; init; }
    public required Guid ActivityUuid { get; init; }
    public required string ActivityName { get; init; }
    public required Guid TenantUuid { get; init; }
    public required string Status { get; init; }
    public int? Score { get; init; }
    public int Attempts { get; init; }
    public int? TimeSpentSeconds { get; init; }
    public string? ProgressData { get; init; }
    public DateOnly? CompletedAt { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public record CreateStudentProgressDto
{
    public required Guid StudentUuid { get; init; }
    public required Guid ActivityUuid { get; init; }
    public required string Status { get; init; }
    public int? Score { get; init; }
    public int Attempts { get; init; }
    public int? TimeSpentSeconds { get; init; }
    public string? ProgressData { get; init; }
    public DateOnly? CompletedAt { get; init; }
}

public record UpdateStudentProgressDto
{
    public string? Status { get; init; }
    public int? Score { get; init; }
    public int? Attempts { get; init; }
    public int? TimeSpentSeconds { get; init; }
    public string? ProgressData { get; init; }
    public DateOnly? CompletedAt { get; init; }
}
