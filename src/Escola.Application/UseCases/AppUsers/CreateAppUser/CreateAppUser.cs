using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.AppUsers.CreateAppUser;

/// <summary>
/// Request to create a new app user.
/// </summary>
public class CreateAppUserRequest
{
    public CreateAppUserDto AppUser { get; set; } = null!;
}

/// <summary>
/// Response containing the created app user.
/// </summary>
public class CreateAppUserResponse
{
    public AppUserDto AppUser { get; set; } = null!;
}

/// <summary>
/// Handler interface for creating app users.
/// </summary>
public interface ICreateAppUserHandler
{
    Task<CreateAppUserResponse> HandleAsync(CreateAppUserRequest request, CancellationToken cancellationToken = default);
}
