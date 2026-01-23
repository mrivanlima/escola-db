using Escola.Application.Services;
using Escola.Application.UseCases.Students.GetStudent;
using Escola.Application.UseCases.Students.GetStudents;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Get Student handler in Infrastructure layer.
/// </summary>
public class GetStudentHandler : IGetStudentHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<StudentResponse?> Handle(Guid studentUuid, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // AsNoTracking for read-only queries
        var student = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentUuid == studentUuid && s.TenantId == currentTenantId.Value && s.DeletedAt == null)
            .Select(s => new StudentResponse
            {
                StudentUuid = s.StudentUuid,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MiddleName = s.MiddleName,
                Nickname = s.Nickname,
                BirthDate = s.BirthDate,
                AvatarConfig = s.AvatarConfig,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return student;
    }
}
