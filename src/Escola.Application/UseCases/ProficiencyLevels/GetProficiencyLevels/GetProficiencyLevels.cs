using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevels;

public record GetProficiencyLevelsRequest
{
    public Guid? TenantUuid { get; init; }
}

public record GetProficiencyLevelsResponse
{
    public List<ProficiencyLevelDto> ProficiencyLevels { get; init; } = new();
}

public interface IGetProficiencyLevelsHandler
{
    Task<GetProficiencyLevelsResponse> HandleAsync(GetProficiencyLevelsRequest request, CancellationToken cancellationToken);
}
