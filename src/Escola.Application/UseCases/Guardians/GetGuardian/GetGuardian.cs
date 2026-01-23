using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Guardians.GetGuardian;

public class GetGuardianRequest
{
    public Guid GuardianUuid { get; set; }
}

public class GetGuardianResponse
{
    public GuardianDto Guardian { get; set; } = null!;
}

public interface IGetGuardianHandler
{
    Task<GetGuardianResponse> HandleAsync(
        GetGuardianRequest request,
        CancellationToken cancellationToken = default);
}
