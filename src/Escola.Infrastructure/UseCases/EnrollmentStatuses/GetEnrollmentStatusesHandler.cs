using Escola.Application.DTOs.School;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatuses;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.EnrollmentStatuses;

public class GetEnrollmentStatusesHandler : IGetEnrollmentStatusesHandler
{
    private readonly EscolaDbContext _context;

    public GetEnrollmentStatusesHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetEnrollmentStatusesResponse> HandleAsync(
        GetEnrollmentStatusesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<EnrollmentStatus>()
            .Include(es => es.Tenant)
            .AsQueryable();

        if (request.TenantUuid.HasValue)
        {
            query = query.Where(es => es.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var enrollmentStatuses = await query
            .OrderBy(es => es.DisplayOrder)
            .ThenBy(es => es.Name)
            .ToListAsync(cancellationToken);

        return new GetEnrollmentStatusesResponse
        {
            EnrollmentStatuses = enrollmentStatuses.Select(es => new EnrollmentStatusDto
            {
                EnrollmentStatusUuid = es.EnrollmentStatusUuid,
                TenantUuid = es.Tenant.TenantUuid,
                TenantName = es.Tenant.TenantName,
                Name = es.Name,
                DisplayOrder = es.DisplayOrder,
                IsActive = es.IsActive
            }).ToList()
        };
    }
}
