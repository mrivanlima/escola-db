using Escola.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escola.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(EscolaDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Basic health check endpoint.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTimeOffset.UtcNow,
            service = "Escola Platform API",
            version = "1.0.0"
        });
    }

    /// <summary>
    /// Database connectivity check.
    /// </summary>
    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Try to connect to the database
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                return Ok(new
                {
                    status = "healthy",
                    database = "connected",
                    timestamp = DateTimeOffset.UtcNow
                });
            }

            return StatusCode(503, new
            {
                status = "unhealthy",
                database = "cannot connect",
                timestamp = DateTimeOffset.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return StatusCode(503, new
            {
                status = "unhealthy",
                database = "error",
                error = ex.Message,
                timestamp = DateTimeOffset.UtcNow
            });
        }
    }
}
