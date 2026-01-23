using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherSubjects.UpdateTeacherSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherSubjects;

public class UpdateTeacherSubjectHandler : IUpdateTeacherSubjectHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTeacherSubjectHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateTeacherSubjectResponse> HandleAsync(UpdateTeacherSubjectRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacherSubject = await _context.TeacherSubjects
            .Include(ts => ts.Teacher).ThenInclude(t => t.User)
            .Include(ts => ts.Subject)
            .Include(ts => ts.ProficiencyLevel)
            .FirstOrDefaultAsync(ts => ts.Teacher.TeacherUuid == request.TeacherUuid && ts.Subject.SubjectUuid == request.SubjectUuid && ts.Teacher.TenantId == currentTenantId.Value, cancellationToken)
            ?? throw new InvalidOperationException($"Teacher subject with TeacherUUID {request.TeacherUuid} and SubjectUUID {request.SubjectUuid} not found or access denied");

        short? proficiencyId = null;
        if (request.TeacherSubject.ProficiencyUuid.HasValue)
        {
            proficiencyId = await _context.ProficiencyLevels
                .Where(p => p.ProficiencyUuid == request.TeacherSubject.ProficiencyUuid.Value && p.DeletedAt == null)
                .Select(p => p.ProficiencyLevelId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (!proficiencyId.HasValue)
                throw new InvalidOperationException($"Proficiency level with UUID {request.TeacherSubject.ProficiencyUuid} not found");
        }

        teacherSubject.ProficiencyLevelId = proficiencyId;
        teacherSubject.YearsExperience = request.TeacherSubject.YearsExperience;
        teacherSubject.IsPrimarySubject = request.TeacherSubject.IsPrimarySubject;
        teacherSubject.Notes = request.TeacherSubject.Notes;
        teacherSubject.UpdatedAt = DateTimeOffset.UtcNow;
        teacherSubject.UpdatedBy = _currentUserService.GetCurrentUserId();

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(teacherSubject).Reference(ts => ts.ProficiencyLevel).LoadAsync(cancellationToken);

        return new UpdateTeacherSubjectResponse
        {
            TeacherSubject = new TeacherSubjectDto
            {
                TeacherUuid = teacherSubject.Teacher.TeacherUuid,
                TeacherName = teacherSubject.Teacher.User.FullName,
                SubjectUuid = teacherSubject.Subject.SubjectUuid,
                SubjectName = teacherSubject.Subject.SubjectName,
                ProficiencyUuid = teacherSubject.ProficiencyLevel?.ProficiencyUuid,
                ProficiencyName = teacherSubject.ProficiencyLevel?.ProficiencyName,
                YearsExperience = teacherSubject.YearsExperience,
                IsPrimarySubject = teacherSubject.IsPrimarySubject,
                Notes = teacherSubject.Notes,
                CreatedAt = teacherSubject.CreatedAt.DateTime,
                UpdatedAt = teacherSubject.UpdatedAt?.DateTime
            }
        };
    }
}
