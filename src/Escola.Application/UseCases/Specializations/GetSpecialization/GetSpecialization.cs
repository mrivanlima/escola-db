using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Specializations.GetSpecialization;

public class GetSpecializationRequest
{
    public Guid SpecializationUuid { get; set; }
}

public class GetSpecializationResponse
{
    public SpecializationDto Specialization { get; set; } = null!;
}

public interface IGetSpecializationHandler
{
    Task<GetSpecializationResponse> HandleAsync(GetSpecializationRequest request, CancellationToken cancellationToken = default);
}
