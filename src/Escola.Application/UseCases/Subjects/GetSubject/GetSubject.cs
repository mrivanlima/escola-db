using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Subjects.GetSubject;

public record GetSubjectRequest
{
    public Guid SubjectUuid { get; init; }
}

public record GetSubjectResponse
{
    public SubjectDto Subject { get; init; } = null!;
}

public interface IGetSubjectHandler
{
    Task<GetSubjectResponse> HandleAsync(GetSubjectRequest request, CancellationToken cancellationToken);
}
