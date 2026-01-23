using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Classes.GetClass;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

public class GetClassHandler : IGetClassHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetClassHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetClassResponse> HandleAsync(GetClassRequest request, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var classEntity = await _context.Set<Class>()
            .Include(c => c.Tenant)
            .Include(c => c.SchoolYear)
            .Include(c => c.GradeLevel)
            .FirstOrDefaultAsync(c => c.ClassUuid == request.ClassUuid && c.TenantId == currentTenantId.Value && c.DeletedAt == null, cancellationToken);

        if (classEntity == null) throw new InvalidOperationException("Class not found or access denied.");

        return new GetClassResponse
        {
            Class = new ClassDto
            {
                ClassUuid = classEntity.ClassUuid,
                TenantUuid = classEntity.Tenant.TenantUuid,
                TenantName = classEntity.Tenant.TenantName,
                ClassName = classEntity.ClassName,
                GradeLevelUuid = classEntity.GradeLevel?.GradeLevelUuid,
                GradeLevelName = classEntity.GradeLevel?.Name,
                SchoolYearUuid = classEntity.SchoolYear.SchoolYearUuid,
                SchoolYearName = classEntity.SchoolYear.Name,
                MaxStudents = classEntity.MaxStudents,
                ClassConfig = classEntity.ClassConfig,
                IsActive = classEntity.IsActive,
                CreatedAt = classEntity.CreatedAt.DateTime,
                UpdatedAt = classEntity.UpdatedAt?.DateTime
            }
        };
    }
}
