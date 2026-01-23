using Escola.Application.UseCases.Health.CheckDatabase;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Escola.Infrastructure.UseCases.Health;

/// <summary>
/// Handler for database health check.
/// </summary>
public class CheckDatabaseHandler : ICheckDatabaseHandler
{
    private readonly EscolaDbContext _context;
    private readonly ILogger<CheckDatabaseHandler> _logger;

    public CheckDatabaseHandler(EscolaDbContext context, ILogger<CheckDatabaseHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Checks database connectivity.
    /// </summary>
    public async Task<CheckDatabaseResponse> Handle(CheckDatabaseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

            return new CheckDatabaseResponse
            {
                Status = canConnect ? "healthy" : "unhealthy",
                Database = canConnect ? "connected" : "cannot connect",
                Timestamp = DateTimeOffset.UtcNow,
                IsConnected = canConnect
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            
            return new CheckDatabaseResponse
            {
                Status = "unhealthy",
                Database = "error",
                Timestamp = DateTimeOffset.UtcNow,
                IsConnected = false
            };
        }
    }
}
