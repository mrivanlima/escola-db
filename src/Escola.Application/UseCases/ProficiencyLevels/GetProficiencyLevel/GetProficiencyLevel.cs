using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevel;

public record GetProficiencyLevelRequest
{
    public Guid ProficiencyUuid { get; init; }
}

public record GetProficiencyLevelResponse
{
    public ProficiencyLevelDto ProficiencyLevel { get; init; } = null!;
}

public interface IGetProficiencyLevelHandler
{
    Task<GetProficiencyLevelResponse> HandleAsync(GetProficiencyLevelRequest request, CancellationToken cancellationToken);
}
