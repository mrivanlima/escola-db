using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Teachers.UpdateTeacher;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

public class UpdateTeacherHandler : IUpdateTeacherHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTeacherHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateTeacherResponse> HandleAsync(UpdateTeacherRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.Teacher;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacher = await _context.Set<Teacher>()
            .Include(t => t.Tenant)
            .Include(t => t.User)
            .Include(t => t.Specialization)
            .FirstOrDefaultAsync(t => t.TeacherUuid == request.TeacherUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null, cancellationToken);

        if (teacher == null) throw new InvalidOperationException("Teacher not found or access denied.");

        if (dto.SpecializationUuid.HasValue)
        {
            var specialization = await _context.Set<Specialization>()
                .FirstOrDefaultAsync(s => s.SpecializationUuid == dto.SpecializationUuid.Value && s.DeletedAt == null, cancellationToken);
            if (specialization == null) throw new InvalidOperationException("Specialization not found.");
            teacher.SpecializationId = specialization.SpecializationId;
            await _context.Entry(teacher).Reference(t => t.Specialization).LoadAsync(cancellationToken);
        }

        if (dto.HireDate.HasValue) teacher.HireDate = dto.HireDate.Value;
        if (dto.IsActive.HasValue) teacher.IsActive = dto.IsActive.Value;

        teacher.UpdatedAt = DateTimeOffset.UtcNow;
        teacher.UpdatedBy = _currentUserService.GetCurrentUserId();
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTeacherResponse
        {
            Teacher = new TeacherDto
            {
                TeacherUuid = teacher.TeacherUuid,
                TenantUuid = teacher.Tenant.TenantUuid,
                TenantName = teacher.Tenant.TenantName,
                UserUuid = teacher.User.UserUuid,
                UserFullName = teacher.User.FullName,
                UserEmail = teacher.User.Email,
                SpecializationUuid = teacher.Specialization?.SpecializationUuid,
                SpecializationName = teacher.Specialization?.SpecializationName,
                HireDate = teacher.HireDate,
                IsActive = teacher.IsActive,
                CreatedAt = teacher.CreatedAt.DateTime,
                UpdatedAt = teacher.UpdatedAt?.DateTime
            }
        };
    }
}
