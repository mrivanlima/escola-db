namespace Escola.Application.UseCases.Health.GetHealth;

/// <summary>
/// Handler interface for basic health check.
/// </summary>
public interface IGetHealthHandler
{
    /// <summary>
    /// Performs basic health check.
    /// </summary>
    /// <param name="request">The health check request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Health check response.</returns>
    Task<GetHealthResponse> Handle(GetHealthRequest request, CancellationToken cancellationToken);
}
