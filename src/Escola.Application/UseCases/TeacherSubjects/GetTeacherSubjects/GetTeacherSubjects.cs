using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherSubjects.GetTeacherSubjects;

public record GetTeacherSubjectsRequest
{
    public Guid? TeacherUuid { get; init; }
    public Guid? SubjectUuid { get; init; }
}

public record GetTeacherSubjectsResponse
{
    public List<TeacherSubjectDto> TeacherSubjects { get; init; } = new();
}

public interface IGetTeacherSubjectsHandler
{
    Task<GetTeacherSubjectsResponse> HandleAsync(GetTeacherSubjectsRequest request, CancellationToken cancellationToken);
}
