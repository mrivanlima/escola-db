using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.TeacherSubjects.CreateTeacherSubject;

public record CreateTeacherSubjectRequest
{
    public CreateTeacherSubjectDto TeacherSubject { get; init; } = null!;
}

public record CreateTeacherSubjectResponse
{
    public TeacherSubjectDto TeacherSubject { get; init; } = null!;
}

public interface ICreateTeacherSubjectHandler
{
    Task<CreateTeacherSubjectResponse> HandleAsync(CreateTeacherSubjectRequest request, CancellationToken cancellationToken);
}
