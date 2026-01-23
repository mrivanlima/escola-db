using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherSubjects.UpdateTeacherSubject;

public record UpdateTeacherSubjectRequest
{
    public Guid TeacherUuid { get; init; }
    public Guid SubjectUuid { get; init; }
    public UpdateTeacherSubjectDto TeacherSubject { get; init; } = null!;
}

public record UpdateTeacherSubjectResponse
{
    public TeacherSubjectDto TeacherSubject { get; init; } = null!;
}

public interface IUpdateTeacherSubjectHandler
{
    Task<UpdateTeacherSubjectResponse> HandleAsync(UpdateTeacherSubjectRequest request, CancellationToken cancellationToken);
}
