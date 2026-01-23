using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Certifications.GetCertifications;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Certifications;

public class GetCertificationsHandler : IGetCertificationsHandler
{
    private readonly EscolaDbContext _context;

    public GetCertificationsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetCertificationsResponse> HandleAsync(GetCertificationsRequest request, CancellationToken cancellationToken)
    {
        var certifications = await _context.Certifications
            .OrderBy(c => c.CertificationName)
            .ToListAsync(cancellationToken);

        return new GetCertificationsResponse
        {
            Certifications = certifications.Select(c => new CertificationDto
            {
                CertificationUuid = c.CertificationUuid,
                CertificationName = c.CertificationName,
                Description = c.Description,
                IssuingOrganization = c.IssuingOrganization,
                IsActive = c.IsActive
            }).ToList()
        };
    }
}
