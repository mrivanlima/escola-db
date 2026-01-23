using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.SchoolYears.UpdateSchoolYear;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.SchoolYears;

public class UpdateSchoolYearHandler : IUpdateSchoolYearHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateSchoolYearHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateSchoolYearResponse> HandleAsync(
        UpdateSchoolYearRequest request,
        CancellationToken cancellationToken = default)
    {
        var dto = request.SchoolYear;

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

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            schoolYear.Name = dto.Name;
            schoolYear.NameNormalized = dto.Name.ToLowerInvariant();
        }

        if (dto.StartDate.HasValue)
        {
            schoolYear.StartDate = dto.StartDate.Value;
        }

        if (dto.EndDate.HasValue)
        {
            schoolYear.EndDate = dto.EndDate.Value;
        }

        // Validate dates after potential updates
        if (schoolYear.EndDate <= schoolYear.StartDate)
        {
            throw new InvalidOperationException("End date must be after start date.");
        }

        if (dto.IsCurrent.HasValue && dto.IsCurrent.Value && !schoolYear.IsCurrent)
        {
            // Unset any existing current year for this tenant
            var existingCurrent = await _context.Set<SchoolYear>()
                .Where(sy => sy.TenantId == schoolYear.TenantId && sy.IsCurrent && sy.SchoolYearId != schoolYear.SchoolYearId && sy.DeletedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var sy in existingCurrent)
            {
                sy.IsCurrent = false;
            }

            schoolYear.IsCurrent = true;
        }
        else if (dto.IsCurrent.HasValue)
        {
            schoolYear.IsCurrent = dto.IsCurrent.Value;
        }

        if (dto.IsActive.HasValue)
        {
            schoolYear.IsActive = dto.IsActive.Value;
        }

        schoolYear.UpdatedAt = DateTimeOffset.UtcNow;
        schoolYear.UpdatedBy = _currentUserService.GetCurrentUserId();

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateSchoolYearResponse
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
