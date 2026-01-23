using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Teachers.CreateTeacher;
using Escola.Domain.Identity;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

public class CreateTeacherHandler : ICreateTeacherHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTeacherHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateTeacherResponse> HandleAsync(CreateTeacherRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.Teacher;

        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // Verify tenant exists and matches current user's tenant
        var tenant = await _context.Set<Tenant>()
            .FirstOrDefaultAsync(t => t.TenantUuid == dto.TenantUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null, cancellationToken);
        if (tenant == null) throw new InvalidOperationException("Tenant not found or access denied.");

        var user = await _context.Set<AppUser>()
            .FirstOrDefaultAsync(u => u.UserUuid == dto.UserUuid && u.DeletedAt == null, cancellationToken);
        if (user == null) throw new InvalidOperationException("User not found.");

        Specialization? specialization = null;
        if (dto.SpecializationUuid.HasValue)
        {
            specialization = await _context.Set<Specialization>()
                .FirstOrDefaultAsync(s => s.SpecializationUuid == dto.SpecializationUuid.Value && s.DeletedAt == null, cancellationToken);
            if (specialization == null) throw new InvalidOperationException("Specialization not found.");
        }

        var teacher = new Teacher
        {
            TeacherUuid = Guid.NewGuid(),
            TenantId = currentTenantId.Value,
            UserId = user.UserId,
            SpecializationId = specialization?.SpecializationId,
            HireDate = dto.HireDate,
            IsActive = dto.IsActive,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.Set<Teacher>().Add(teacher);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(teacher).Reference(t => t.Tenant).LoadAsync(cancellationToken);
        await _context.Entry(teacher).Reference(t => t.User).LoadAsync(cancellationToken);
        if (teacher.SpecializationId.HasValue)
            await _context.Entry(teacher).Reference(t => t.Specialization).LoadAsync(cancellationToken);

        return new CreateTeacherResponse
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
