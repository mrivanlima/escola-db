using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Certifications.GetCertification;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Certifications;

public class GetCertificationHandler : IGetCertificationHandler
{
    private readonly EscolaDbContext _context;

    public GetCertificationHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetCertificationResponse> HandleAsync(GetCertificationRequest request, CancellationToken cancellationToken)
    {
        var certification = await _context.Certifications
            .FirstOrDefaultAsync(c => c.CertificationUuid == request.CertificationUuid, cancellationToken)
            ?? throw new InvalidOperationException($"Certification with UUID {request.CertificationUuid} not found");

        return new GetCertificationResponse
        {
            Certification = new CertificationDto
            {
                CertificationUuid = certification.CertificationUuid,
                CertificationName = certification.CertificationName,
                Description = certification.Description,
                IssuingOrganization = certification.IssuingOrganization,
                IsActive = certification.IsActive
            }
        };
    }
}
