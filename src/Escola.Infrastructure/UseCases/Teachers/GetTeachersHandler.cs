using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Teachers;

public class GetTeachersHandler : IGetTeachersHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeachersHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeachersResponse> HandleAsync(GetTeachersRequest request, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.Set<Teacher>()
            .Include(t => t.Tenant)
            .Include(t => t.User)
            .Include(t => t.Specialization)
            .Where(t => t.TenantId == currentTenantId.Value && t.DeletedAt == null);

        if (request.TenantUuid.HasValue)
            query = query.Where(t => t.Tenant.TenantUuid == request.TenantUuid.Value);

        if (request.SpecializationUuid.HasValue)
            query = query.Where(t => t.Specialization != null && t.Specialization.SpecializationUuid == request.SpecializationUuid.Value);

        var teachers = await query
            .OrderBy(t => t.User.FullName)
            .ToListAsync(cancellationToken);

        return new GetTeachersResponse
        {
            Teachers = teachers.Select(t => new TeacherDto
            {
                TeacherUuid = t.TeacherUuid,
                TenantUuid = t.Tenant.TenantUuid,
                TenantName = t.Tenant.TenantName,
                UserUuid = t.User.UserUuid,
                UserFullName = t.User.FullName,
                UserEmail = t.User.Email,
                SpecializationUuid = t.Specialization?.SpecializationUuid,
                SpecializationName = t.Specialization?.SpecializationName,
                HireDate = t.HireDate,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt.DateTime,
                UpdatedAt = t.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
