using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Guardians.CreateGuardian;

public class CreateGuardianRequest
{
    public CreateGuardianDto Guardian { get; set; } = null!;
}

public class CreateGuardianResponse
{
    public GuardianDto Guardian { get; set; } = null!;
}

public interface ICreateGuardianHandler
{
    Task<CreateGuardianResponse> HandleAsync(
        CreateGuardianRequest request,
        CancellationToken cancellationToken = default);
}
