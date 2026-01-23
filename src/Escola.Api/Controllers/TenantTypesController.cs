using Escola.Api.Common;
using Escola.Application.UseCases.TenantTypes.GetTenantType;
using Escola.Application.UseCases.TenantTypes.GetTenantTypes;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Tenant Types management endpoints (Read-only lookup table).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantTypesController : ControllerBase
{
    private readonly IGetTenantTypesHandler _getTenantTypesHandler;
    private readonly IGetTenantTypeHandler _getTenantTypeHandler;
    private readonly ILogger<TenantTypesController> _logger;

    public TenantTypesController(
        IGetTenantTypesHandler getTenantTypesHandler,
        IGetTenantTypeHandler getTenantTypeHandler,
        ILogger<TenantTypesController> logger)
    {
        _getTenantTypesHandler = getTenantTypesHandler;
        _getTenantTypeHandler = getTenantTypeHandler;
        _logger = logger;
    }

    /// <summary>
    /// Get all tenant types.
    /// </summary>
    /// <returns>List of all tenant types in the system.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetTenantTypesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTenantTypesRequest();
            var response = await _getTenantTypesHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<GetTenantTypesResponse>.SuccessResult(
                response,
                $"Retrieved {response.TenantTypes.Count} tenant types"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenant types");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching tenant types",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Get a single tenant type by UUID.
    /// </summary>
    /// <param name="uuid">External UUID of the tenant type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tenant type details.</returns>
    [HttpGet("{uuid:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetTenantTypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTenantTypeRequest { TenantTypeUuid = uuid };
            var response = await _getTenantTypeHandler.Handle(request, cancellationToken);

            if (response.TenantType == null)
            {
                return NotFound(ApiResponse<object>.FailureResult($"Tenant type with UUID {uuid} not found"));
            }

            return Ok(ApiResponse<GetTenantTypeResponse>.SuccessResult(response, "Tenant type retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenant type {TenantTypeUuid}", uuid);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching the tenant type",
                new List<string> { ex.Message }));
        }
    }
}
