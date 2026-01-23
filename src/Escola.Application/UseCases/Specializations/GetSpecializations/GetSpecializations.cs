using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Specializations.GetSpecializations;

public class GetSpecializationsRequest { }

public class GetSpecializationsResponse
{
    public List<SpecializationDto> Specializations { get; set; } = new();
}

public interface IGetSpecializationsHandler
{
    Task<GetSpecializationsResponse> HandleAsync(GetSpecializationsRequest request, CancellationToken cancellationToken = default);
}
