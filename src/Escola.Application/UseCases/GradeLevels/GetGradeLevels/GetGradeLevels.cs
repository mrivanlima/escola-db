using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.GradeLevels.GetGradeLevels;

/// <summary>
/// Request to get all grade levels (optionally filtered by tenant).
/// </summary>
public class GetGradeLevelsRequest
{
    /// <summary>
    /// Optional tenant UUID to filter grade levels.
    /// If not provided, returns all grade levels (subject to user permissions).
    /// </summary>
    public Guid? TenantUuid { get; set; }
}

/// <summary>
/// Response containing the list of grade levels.
/// </summary>
public class GetGradeLevelsResponse
{
    public List<GradeLevelDto> GradeLevels { get; set; } = new();
}

/// <summary>
/// Handler interface for getting all grade levels.
/// </summary>
public interface IGetGradeLevelsHandler
{
    Task<GetGradeLevelsResponse> HandleAsync(GetGradeLevelsRequest request, CancellationToken cancellationToken = default);
}
