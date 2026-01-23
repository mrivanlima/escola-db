using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Subjects.GetSubjects;

public record GetSubjectsRequest
{
    public Guid? GradeLevelUuid { get; init; }
}

public record GetSubjectsResponse
{
    public List<SubjectDto> Subjects { get; init; } = new();
}

public interface IGetSubjectsHandler
{
    Task<GetSubjectsResponse> HandleAsync(GetSubjectsRequest request, CancellationToken cancellationToken);
}
