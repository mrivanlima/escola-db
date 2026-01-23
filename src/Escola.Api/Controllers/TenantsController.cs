using Escola.Api.Common;
using Escola.Application.UseCases.Tenants.GetTenants;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Tenants management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IGetTenantsHandler _getTenantsHandler;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(IGetTenantsHandler getTenantsHandler, ILogger<TenantsController> logger)
    {
        _getTenantsHandler = getTenantsHandler;
        _logger = logger;
    }

    /// <summary>
    /// Get all tenants.
    /// </summary>
    /// <returns>List of all tenants in the system.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTenantsRequest();
            var response = await _getTenantsHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<GetTenantsResponse>.SuccessResult(
                response,
                $"Retrieved {response.Tenants.Count} tenants"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenants");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching tenants",
                new List<string> { ex.Message }));
        }
    }
}
