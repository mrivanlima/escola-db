using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.AppUsers.GetAppUsers;

/// <summary>
/// Request to get all app users (optionally filtered by tenant).
/// </summary>
public class GetAppUsersRequest
{
    /// <summary>
    /// Optional tenant UUID to filter users.
    /// If not provided, returns all users (subject to user permissions).
    /// </summary>
    public Guid? TenantUuid { get; set; }
}

/// <summary>
/// Response containing the list of app users.
/// </summary>
public class GetAppUsersResponse
{
    public List<AppUserDto> AppUsers { get; set; } = new();
}

/// <summary>
/// Handler interface for getting all app users.
/// </summary>
public interface IGetAppUsersHandler
{
    Task<GetAppUsersResponse> HandleAsync(GetAppUsersRequest request, CancellationToken cancellationToken = default);
}
