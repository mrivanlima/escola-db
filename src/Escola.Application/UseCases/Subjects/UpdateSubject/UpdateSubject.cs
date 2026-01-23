using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Subjects.UpdateSubject;

public record UpdateSubjectRequest
{
    public Guid SubjectUuid { get; init; }
    public UpdateSubjectDto Subject { get; init; } = null!;
}

public record UpdateSubjectResponse
{
    public SubjectDto Subject { get; init; } = null!;
}

public interface IUpdateSubjectHandler
{
    Task<UpdateSubjectResponse> HandleAsync(UpdateSubjectRequest request, CancellationToken cancellationToken);
}
