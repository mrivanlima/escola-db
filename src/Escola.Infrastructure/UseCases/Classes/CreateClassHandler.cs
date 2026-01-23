using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Classes.CreateClass;
using Escola.Domain.Identity;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

public class CreateClassHandler : ICreateClassHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateClassHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateClassResponse> HandleAsync(CreateClassRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.Class;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // Validate tenant exists and matches current user's tenant
        var tenant = await _context.Set<Tenant>()
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null, cancellationToken);
        if (tenant == null) throw new InvalidOperationException("Tenant not found or access denied.");

        var schoolYear = await _context.Set<SchoolYear>()
            .FirstOrDefaultAsync(sy => sy.SchoolYearUuid == dto.SchoolYearUuid && sy.TenantId == currentTenantId.Value && sy.DeletedAt == null, cancellationToken);
        if (schoolYear == null) throw new InvalidOperationException("School year not found or access denied.");

        GradeLevel? gradeLevel = null;
        if (dto.GradeLevelUuid.HasValue)
        {
            gradeLevel = await _context.Set<GradeLevel>()
                .FirstOrDefaultAsync(gl => gl.GradeLevelUuid == dto.GradeLevelUuid.Value && gl.DeletedAt == null, cancellationToken);
            if (gradeLevel == null) throw new InvalidOperationException("Grade level not found.");
        }

        var classEntity = new Class
        {
            ClassUuid = Guid.NewGuid(),
            TenantId = currentTenantId.Value,
            ClassName = dto.ClassName,
            ClassNameNormalized = dto.ClassName.ToLowerInvariant(),
            GradeLevelId = gradeLevel?.GradeLevelId,
            SchoolYearId = schoolYear.SchoolYearId,
            MaxStudents = dto.MaxStudents,
            ClassConfig = dto.ClassConfig,
            IsActive = dto.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.Set<Class>().Add(classEntity);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(classEntity).Reference(c => c.Tenant).LoadAsync(cancellationToken);
        await _context.Entry(classEntity).Reference(c => c.SchoolYear).LoadAsync(cancellationToken);
        if (classEntity.GradeLevelId.HasValue)
            await _context.Entry(classEntity).Reference(c => c.GradeLevel).LoadAsync(cancellationToken);

        return new CreateClassResponse
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
