using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.SchoolYears.CreateSchoolYear;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.SchoolYears;

public class CreateSchoolYearHandler : ICreateSchoolYearHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateSchoolYearHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateSchoolYearResponse> HandleAsync(
        CreateSchoolYearRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.SchoolYear;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // Validate tenant exists and matches current user's tenant
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException("Tenant not found or access denied.");
        }

        // If setting as current, unset any existing current year for this tenant
        if (dto.IsCurrent)
        {
            var existingCurrent = await _context.Set<SchoolYear>()
                .Where(sy => sy.TenantId == currentTenantId.Value && sy.IsCurrent && sy.DeletedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var sy in existingCurrent)
            {
                sy.IsCurrent = false;
            }
        }

        var schoolYear = new SchoolYear
        {
            SchoolYearUuid = Guid.NewGuid(),
            TenantId = currentTenantId.Value,
            Name = dto.Name,
            NameNormalized = dto.Name.ToLowerInvariant(),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsCurrent = dto.IsCurrent,
            IsActive = dto.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.Set<SchoolYear>().Add(schoolYear);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(schoolYear)
            .Reference(sy => sy.Tenant)
            .LoadAsync(cancellationToken);

        return new CreateSchoolYearResponse
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
