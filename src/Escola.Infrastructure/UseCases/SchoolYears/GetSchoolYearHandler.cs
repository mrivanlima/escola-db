using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.SchoolYears.GetSchoolYear;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.SchoolYears;

public class GetSchoolYearHandler : IGetSchoolYearHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSchoolYearHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetSchoolYearResponse> HandleAsync(
        GetSchoolYearRequest request,
        CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var schoolYear = await _context.Set<SchoolYear>()
            .Include(sy => sy.Tenant)
            .FirstOrDefaultAsync(sy => sy.SchoolYearUuid == request.SchoolYearUuid && sy.TenantId == currentTenantId.Value && sy.DeletedAt == null, cancellationToken);

        if (schoolYear == null)
        {
            throw new InvalidOperationException("School year not found or access denied.");
        }

        return new GetSchoolYearResponse
        {
            SchoolYear = new SchoolYearDto
            {
                SchoolYearUuid = schoolYear.SchoolYearUuid,
                TenantUuid = schoolYear.Tenant.TenantUuid,
                TenantName = schoolYear.Tenant.TenantName,
                Name = schoolYear.Name,
                StartDate = schoolYear.StartDate,
                EndDate = schoolYear.EndDate,
                IsCurrent = schoolYear.IsCurrent,
                IsActive = schoolYear.IsActive,
                CreatedAt = schoolYear.CreatedAt.DateTime,
                UpdatedAt = schoolYear.UpdatedAt?.DateTime
            }
        };
    }
}
