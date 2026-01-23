using Escola.Application.Services;
using Escola.Application.UseCases.Students.DeleteStudent;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Delete Student handler in Infrastructure layer.
/// Uses soft delete pattern - sets DeletedAt and IsActive = false.
/// </summary>
public class DeleteStudentHandler : IDeleteStudentHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteStudentHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(Guid studentUuid, CancellationToken cancellationToken = default)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        // Find the student by UUID with tenant filtering
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentUuid == studentUuid && s.TenantId == currentTenantId.Value && s.DeletedAt == null, cancellationToken);

        if (student == null)
        {
            return false;
        }

        // Soft delete: Set DeletedAt and IsActive = false
        // Do NOT use _context.Students.Remove() - that would be a hard delete
        student.DeletedAt = DateTimeOffset.UtcNow;
        student.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
