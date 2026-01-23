using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.SchoolYears.GetSchoolYears;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.SchoolYears;

public class GetSchoolYearsHandler : IGetSchoolYearsHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSchoolYearsHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetSchoolYearsResponse> HandleAsync(
        GetSchoolYearsRequest request,
        CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.Set<SchoolYear>()
            .Include(sy => sy.Tenant)
            .Where(sy => sy.TenantId == currentTenantId.Value && sy.DeletedAt == null);

        if (request.TenantUuid.HasValue)
        {
            query = query.Where(sy => sy.Tenant.TenantUuid == request.TenantUuid.Value);
        }

        var schoolYears = await query
            .OrderByDescending(sy => sy.StartDate)
            .ToListAsync(cancellationToken);

        return new GetSchoolYearsResponse
        {
            SchoolYears = schoolYears.Select(sy => new SchoolYearDto
            {
                SchoolYearUuid = sy.SchoolYearUuid,
                TenantUuid = sy.Tenant.TenantUuid,
                TenantName = sy.Tenant.TenantName,
                Name = sy.Name,
                StartDate = sy.StartDate,
                EndDate = sy.EndDate,
                IsCurrent = sy.IsCurrent,
                IsActive = sy.IsActive,
                CreatedAt = sy.CreatedAt.DateTime,
                UpdatedAt = sy.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
