using Escola.Api.Common;
using Escola.Application.UseCases.GradeLevels.GetGradeLevel;
using Escola.Application.UseCases.GradeLevels.GetGradeLevels;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Controller for managing grade levels.
/// Read-only lookup table.
/// </summary>
[ApiController]
[Route("api/gradelevels")]
public class GradeLevelsController : ControllerBase
{
    private readonly IGetGradeLevelsHandler _getGradeLevelsHandler;
    private readonly IGetGradeLevelHandler _getGradeLevelHandler;

    public GradeLevelsController(
        IGetGradeLevelsHandler getGradeLevelsHandler,
        IGetGradeLevelHandler getGradeLevelHandler)
    {
        _getGradeLevelsHandler = getGradeLevelsHandler;
        _getGradeLevelHandler = getGradeLevelHandler;
    }

    /// <summary>
    /// Get all grade levels, optionally filtered by tenant.
    /// </summary>
    /// <param name="tenantUuid">Optional tenant UUID to filter grade levels.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of grade levels.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetGradeLevelsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradeLevels(
        [FromQuery] Guid? tenantUuid,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetGradeLevelsRequest { TenantUuid = tenantUuid };
            var result = await _getGradeLevelsHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GetGradeLevelsResponse>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GetGradeLevelsResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get a single grade level by UUID.
    /// </summary>
    /// <param name="uuid">The grade level UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested grade level.</returns>
    [HttpGet("{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<GetGradeLevelResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<GetGradeLevelResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradeLevel(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetGradeLevelRequest { GradeLevelUuid = uuid };
            var result = await _getGradeLevelHandler.HandleAsync(request, cancellationToken);
            return Ok(ApiResponse<GetGradeLevelResponse>.SuccessResult(result));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ApiResponse<GetGradeLevelResponse>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<GetGradeLevelResponse>.FailureResult($"An error occurred: {ex.Message}"));
        }
    }
}
