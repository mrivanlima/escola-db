using Escola.Application.UseCases.Health.GetHealth;

namespace Escola.Infrastructure.UseCases.Health;

/// <summary>
/// Handler for basic health check.
/// </summary>
public class GetHealthHandler : IGetHealthHandler
{
    /// <summary>
    /// Performs basic health check.
    /// </summary>
    public Task<GetHealthResponse> Handle(GetHealthRequest request, CancellationToken cancellationToken)
    {
        var response = new GetHealthResponse
        {
            Status = "healthy",
            Timestamp = DateTimeOffset.UtcNow,
            Service = "Escola Platform API",
            Version = "1.0.0"
        };

        return Task.FromResult(response);
    }
}
