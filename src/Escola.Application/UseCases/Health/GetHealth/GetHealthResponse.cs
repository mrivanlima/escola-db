namespace Escola.Application.UseCases.Health.GetHealth;

/// <summary>
/// Response containing basic health information.
/// </summary>
public class GetHealthResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public string Service { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
