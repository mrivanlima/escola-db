using Escola.Application.Services;
using Escola.Application.UseCases.Students.GetStudents;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Get Students handler in Infrastructure layer.
/// </summary>
public class GetStudentsHandler : IGetStudentsHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetStudentsHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<StudentResponse>> Handle(GetStudentsRequest request, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // AsNoTracking for read-only queries with tenant filtering
        var students = await _context.Students
            .AsNoTracking()
            .Where(s => s.TenantId == currentTenantId.Value && s.DeletedAt == null)
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
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
            .ToListAsync(cancellationToken);

        return students;
    }
}
