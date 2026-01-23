using Escola.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escola.Api.Controllers;

/// <summary>
/// Tenants management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(EscolaDbContext context, ILogger<TenantsController> logger)
    {
        _context = context;
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
            _logger.LogInformation("Fetching all tenants");

            var tenants = await _context.Tenants
                .OrderBy(t => t.TenantName)
                .Select(t => new
                {
                    t.TenantId,
                    t.TenantUuid,
                    t.TenantName,
                    t.TenantType,
                    t.IsActive,
                    t.CreatedAt,
                    t.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} tenants", tenants.Count);

            return Ok(new
            {
                success = true,
                count = tenants.Count,
                data = tenants
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tenants");
            return StatusCode(500, new
            {
                success = false,
                error = "An error occurred while fetching tenants",
                detail = ex.Message
            });
        }
    }
}
