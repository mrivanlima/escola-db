using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.AppUsers.GetAppUser;

/// <summary>
/// Request to get a single app user by UUID.
/// </summary>
public class GetAppUserRequest
{
    public Guid UserUuid { get; set; }
}

/// <summary>
/// Response containing the requested app user.
/// </summary>
public class GetAppUserResponse
{
    public AppUserDto AppUser { get; set; } = null!;
}

/// <summary>
/// Handler interface for getting a single app user.
/// </summary>
public interface IGetAppUserHandler
{
    Task<GetAppUserResponse> HandleAsync(GetAppUserRequest request, CancellationToken cancellationToken = default);
}
