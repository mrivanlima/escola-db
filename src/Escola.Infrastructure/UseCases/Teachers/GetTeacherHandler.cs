using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

public class GetTeacherHandler : IGetTeacherHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeacherHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeacherResponse> HandleAsync(GetTeacherRequest request, CancellationToken cancellationToken = default)
    {
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

        return new GetTeacherResponse
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
