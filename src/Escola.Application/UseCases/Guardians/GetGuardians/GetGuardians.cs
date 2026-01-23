using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Guardians.GetGuardians;

public class GetGuardiansRequest
{
    public Guid? TenantUuid { get; set; }
}

public class GetGuardiansResponse
{
    public List<GuardianDto> Guardians { get; set; } = new();
}

public interface IGetGuardiansHandler
{
    Task<GetGuardiansResponse> HandleAsync(
        GetGuardiansRequest request,
        CancellationToken cancellationToken = default);
}
