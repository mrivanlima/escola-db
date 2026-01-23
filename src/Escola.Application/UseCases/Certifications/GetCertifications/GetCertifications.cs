using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Certifications.GetCertifications;

public record GetCertificationsRequest
{
}

public record GetCertificationsResponse
{
    public List<CertificationDto> Certifications { get; init; } = new();
}

public interface IGetCertificationsHandler
{
    Task<GetCertificationsResponse> HandleAsync(GetCertificationsRequest request, CancellationToken cancellationToken);
}
