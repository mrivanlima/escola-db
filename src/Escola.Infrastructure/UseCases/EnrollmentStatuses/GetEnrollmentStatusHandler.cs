using Escola.Application.DTOs.School;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatus;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.EnrollmentStatuses;

public class GetEnrollmentStatusHandler : IGetEnrollmentStatusHandler
{
    private readonly EscolaDbContext _context;

    public GetEnrollmentStatusHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetEnrollmentStatusResponse> HandleAsync(
        GetEnrollmentStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var enrollmentStatus = await _context.Set<EnrollmentStatus>()
            .Include(es => es.Tenant)
            .FirstOrDefaultAsync(es => es.EnrollmentStatusUuid == request.EnrollmentStatusUuid, cancellationToken);

        if (enrollmentStatus == null)
        {
            throw new InvalidOperationException("Enrollment status not found.");
        }

        return new GetEnrollmentStatusResponse
        {
            EnrollmentStatus = new EnrollmentStatusDto
            {
                EnrollmentStatusUuid = enrollmentStatus.EnrollmentStatusUuid,
                TenantUuid = enrollmentStatus.Tenant.TenantUuid,
                TenantName = enrollmentStatus.Tenant.TenantName,
                Name = enrollmentStatus.Name,
                DisplayOrder = enrollmentStatus.DisplayOrder,
                IsActive = enrollmentStatus.IsActive
            }
        };
    }
}
