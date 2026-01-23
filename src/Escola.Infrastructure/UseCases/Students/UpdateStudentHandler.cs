using Escola.Application.Services;
using Escola.Application.UseCases.Students.UpdateStudent;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Students;

/// <summary>
/// Implementation of Update Student handler in Infrastructure layer.
/// </summary>
public class UpdateStudentHandler : IUpdateStudentHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateStudentHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateStudentResponse?> Handle(Guid studentUuid, UpdateStudentRequest request, CancellationToken cancellationToken = default)
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
            return null;
        }

        // Update the student properties
        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.MiddleName = request.MiddleName;
        student.Nickname = request.Nickname;
        student.BirthDate = request.BirthDate;
        student.UpdatedAt = DateTimeOffset.UtcNow;
        student.UpdatedBy = _currentUserService.GetCurrentUserId();

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateStudentResponse
        {
            StudentUuid = student.StudentUuid,
            FirstName = student.FirstName,
            LastName = student.LastName,
            MiddleName = student.MiddleName,
            Nickname = student.Nickname,
            BirthDate = student.BirthDate,
            UpdatedAt = student.UpdatedAt
        };
    }
}
