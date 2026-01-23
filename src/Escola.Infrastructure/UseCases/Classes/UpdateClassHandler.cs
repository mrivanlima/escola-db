using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Classes.UpdateClass;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Classes;

public class UpdateClassHandler : IUpdateClassHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateClassHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateClassResponse> HandleAsync(UpdateClassRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.Class;

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

        if (!string.IsNullOrWhiteSpace(dto.ClassName))
        {
            classEntity.ClassName = dto.ClassName;
            classEntity.ClassNameNormalized = dto.ClassName.ToLowerInvariant();
        }

        if (dto.GradeLevelUuid.HasValue)
        {
            var gradeLevel = await _context.Set<GradeLevel>()
                .FirstOrDefaultAsync(gl => gl.GradeLevelUuid == dto.GradeLevelUuid.Value && gl.DeletedAt == null, cancellationToken);
            if (gradeLevel == null) throw new InvalidOperationException("Grade level not found.");
            classEntity.GradeLevelId = gradeLevel.GradeLevelId;
            await _context.Entry(classEntity).Reference(c => c.GradeLevel).LoadAsync(cancellationToken);
        }

        if (dto.SchoolYearUuid.HasValue)
        {
            var schoolYear = await _context.Set<SchoolYear>()
                .FirstOrDefaultAsync(sy => sy.SchoolYearUuid == dto.SchoolYearUuid.Value && sy.TenantId == currentTenantId.Value && sy.DeletedAt == null, cancellationToken);
            if (schoolYear == null) throw new InvalidOperationException("School year not found or access denied.");
            classEntity.SchoolYearId = schoolYear.SchoolYearId;
            await _context.Entry(classEntity).Reference(c => c.SchoolYear).LoadAsync(cancellationToken);
        }

        if (dto.MaxStudents.HasValue) classEntity.MaxStudents = dto.MaxStudents.Value;
        if (dto.ClassConfig != null) classEntity.ClassConfig = dto.ClassConfig;
        if (dto.IsActive.HasValue) classEntity.IsActive = dto.IsActive.Value;

        classEntity.UpdatedAt = DateTimeOffset.UtcNow;
        classEntity.UpdatedBy = _currentUserService.GetCurrentUserId();
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateClassResponse
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
