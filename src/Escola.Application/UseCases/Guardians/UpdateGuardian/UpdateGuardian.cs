using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Guardians.UpdateGuardian;

public class UpdateGuardianRequest
{
    public Guid GuardianUuid { get; set; }
    public UpdateGuardianDto Guardian { get; set; } = null!;
}

public class UpdateGuardianResponse
{
    public GuardianDto Guardian { get; set; } = null!;
}

public interface IUpdateGuardianHandler
{
    Task<UpdateGuardianResponse> HandleAsync(
        UpdateGuardianRequest request,
        CancellationToken cancellationToken = default);
}
