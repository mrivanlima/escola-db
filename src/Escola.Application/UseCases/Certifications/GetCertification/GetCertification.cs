using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Certifications.GetCertification;

public record GetCertificationRequest
{
    public Guid CertificationUuid { get; init; }
}

public record GetCertificationResponse
{
    public CertificationDto Certification { get; init; } = null!;
}

public interface IGetCertificationHandler
{
    Task<GetCertificationResponse> HandleAsync(GetCertificationRequest request, CancellationToken cancellationToken);
}
