using Escola.Application.DTOs.Identity;

namespace Escola.Application.UseCases.AppUsers.UpdateAppUser;

/// <summary>
/// Request to update an existing app user.
/// </summary>
public class UpdateAppUserRequest
{
    public Guid UserUuid { get; set; }
    public UpdateAppUserDto AppUser { get; set; } = null!;
}

/// <summary>
/// Response containing the updated app user.
/// </summary>
public class UpdateAppUserResponse
{
    public AppUserDto AppUser { get; set; } = null!;
}

/// <summary>
/// Handler interface for updating app users.
/// </summary>
public interface IUpdateAppUserHandler
{
    Task<UpdateAppUserResponse> HandleAsync(UpdateAppUserRequest request, CancellationToken cancellationToken = default);
}
