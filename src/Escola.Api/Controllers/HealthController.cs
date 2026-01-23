using Escola.Api.Common;
using Escola.Application.UseCases.Health.CheckDatabase;
using Escola.Application.UseCases.Health.GetHealth;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IGetHealthHandler _getHealthHandler;
    private readonly ICheckDatabaseHandler _checkDatabaseHandler;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        IGetHealthHandler getHealthHandler,
        ICheckDatabaseHandler checkDatabaseHandler,
        ILogger<HealthController> logger)
    {
        _getHealthHandler = getHealthHandler;
        _checkDatabaseHandler = checkDatabaseHandler;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check endpoint.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var request = new GetHealthRequest();
        var response = await _getHealthHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<GetHealthResponse>.SuccessResult(response, "Service is healthy"));
    }

    /// <summary>
    /// Database connectivity check.
    /// </summary>
    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase(CancellationToken cancellationToken)
    {
        try
        {
            var request = new CheckDatabaseRequest();
            var response = await _checkDatabaseHandler.Handle(request, cancellationToken);

            if (response.IsConnected)
            {
                return Ok(ApiResponse<CheckDatabaseResponse>.SuccessResult(response, "Database is connected"));
            }

            return StatusCode(503, ApiResponse<CheckDatabaseResponse>.FailureResult(
                "Database connection failed"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return StatusCode(503, ApiResponse<object>.FailureResult(
                "Database health check error",
                new List<string> { ex.Message }));
        }
    }
}
