using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.GradeLevels.GetGradeLevel;

/// <summary>
/// Request to get a single grade level by UUID.
/// </summary>
public class GetGradeLevelRequest
{
    public Guid GradeLevelUuid { get; set; }
}

/// <summary>
/// Response containing the requested grade level.
/// </summary>
public class GetGradeLevelResponse
{
    public GradeLevelDto GradeLevel { get; set; } = null!;
}

/// <summary>
/// Handler interface for getting a single grade level.
/// </summary>
public interface IGetGradeLevelHandler
{
    Task<GetGradeLevelResponse> HandleAsync(GetGradeLevelRequest request, CancellationToken cancellationToken = default);
}
