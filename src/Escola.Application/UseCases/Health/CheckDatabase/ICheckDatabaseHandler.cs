namespace Escola.Application.UseCases.Health.CheckDatabase;

/// <summary>
/// Handler interface for database health check.
/// </summary>
public interface ICheckDatabaseHandler
{
    /// <summary>
    /// Checks database connectivity and health.
    /// </summary>
    /// <param name="request">The database check request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Database health check response.</returns>
    Task<CheckDatabaseResponse> Handle(CheckDatabaseRequest request, CancellationToken cancellationToken);
}
