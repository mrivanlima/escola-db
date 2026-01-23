using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Subjects.CreateSubject;

public record CreateSubjectRequest
{
    public CreateSubjectDto Subject { get; init; } = null!;
}

public record CreateSubjectResponse
{
    public SubjectDto Subject { get; init; } = null!;
}

public interface ICreateSubjectHandler
{
    Task<CreateSubjectResponse> HandleAsync(CreateSubjectRequest request, CancellationToken cancellationToken);
}
