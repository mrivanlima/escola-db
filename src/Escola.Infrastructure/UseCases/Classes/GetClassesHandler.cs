using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

public class GetClassesHandler : IGetClassesHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetClassesHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetClassesResponse> HandleAsync(GetClassesRequest request, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.Set<Class>()
            .Include(c => c.Tenant)
            .Include(c => c.SchoolYear)
            .Include(c => c.GradeLevel)
            .Where(c => c.TenantId == currentTenantId.Value && c.DeletedAt == null);

        if (request.TenantUuid.HasValue)
            query = query.Where(c => c.Tenant.TenantUuid == request.TenantUuid.Value);

        if (request.SchoolYearUuid.HasValue)
            query = query.Where(c => c.SchoolYear.SchoolYearUuid == request.SchoolYearUuid.Value);

        var classes = await query.OrderBy(c => c.ClassName).ToListAsync(cancellationToken);

        return new GetClassesResponse
        {
            Classes = classes.Select(c => new ClassDto
            {
                ClassUuid = c.ClassUuid,
                TenantUuid = c.Tenant.TenantUuid,
                TenantName = c.Tenant.TenantName,
                ClassName = c.ClassName,
                GradeLevelUuid = c.GradeLevel?.GradeLevelUuid,
                GradeLevelName = c.GradeLevel?.Name,
                SchoolYearUuid = c.SchoolYear.SchoolYearUuid,
                SchoolYearName = c.SchoolYear.Name,
                MaxStudents = c.MaxStudents,
                ClassConfig = c.ClassConfig,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt.DateTime,
                UpdatedAt = c.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
