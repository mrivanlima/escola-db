using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherSubjects.GetTeacherSubject;

public record GetTeacherSubjectRequest
{
    public Guid TeacherUuid { get; init; }
    public Guid SubjectUuid { get; init; }
}

public record GetTeacherSubjectResponse
{
    public TeacherSubjectDto TeacherSubject { get; init; } = null!;
}

public interface IGetTeacherSubjectHandler
{
    Task<GetTeacherSubjectResponse> HandleAsync(GetTeacherSubjectRequest request, CancellationToken cancellationToken);
}
