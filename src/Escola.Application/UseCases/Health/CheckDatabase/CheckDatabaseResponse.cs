namespace Escola.Application.UseCases.Health.CheckDatabase;

/// <summary>
/// Response containing database health information.
/// </summary>
public class CheckDatabaseResponse
{
    public string Status { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public bool IsConnected { get; set; }
}
