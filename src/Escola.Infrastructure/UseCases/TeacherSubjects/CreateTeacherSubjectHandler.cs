using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherSubjects.CreateTeacherSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherSubjects;

public class CreateTeacherSubjectHandler : ICreateTeacherSubjectHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTeacherSubjectHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateTeacherSubjectResponse> HandleAsync(CreateTeacherSubjectRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacherId = await _context.Teachers
            .Where(t => t.TeacherUuid == request.TeacherSubject.TeacherUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null)
            .Select(t => t.TeacherId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (teacherId == 0)
            throw new InvalidOperationException($"Teacher with UUID {request.TeacherSubject.TeacherUuid} not found or access denied");

        var subjectId = await _context.Subjects
            .Where(s => s.SubjectUuid == request.TeacherSubject.SubjectUuid && s.DeletedAt == null)
            .Select(s => s.SubjectId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (subjectId == 0)
            throw new InvalidOperationException($"Subject with UUID {request.TeacherSubject.SubjectUuid} not found");

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

        var teacherSubject = new TeacherSubject
        {
            TeacherId = teacherId,
            SubjectId = subjectId,
            ProficiencyLevelId = proficiencyId,
            YearsExperience = request.TeacherSubject.YearsExperience,
            IsPrimarySubject = request.TeacherSubject.IsPrimarySubject,
            Notes = request.TeacherSubject.Notes,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.TeacherSubjects.Add(teacherSubject);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.TeacherSubjects
            .Include(ts => ts.Teacher).ThenInclude(t => t.User)
            .Include(ts => ts.Subject)
            .Include(ts => ts.ProficiencyLevel)
            .FirstAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId, cancellationToken);

        return new CreateTeacherSubjectResponse
        {
            TeacherSubject = new TeacherSubjectDto
            {
                TeacherUuid = result.Teacher.TeacherUuid,
                TeacherName = result.Teacher.User.FullName,
                SubjectUuid = result.Subject.SubjectUuid,
                SubjectName = result.Subject.SubjectName,
                ProficiencyUuid = result.ProficiencyLevel?.ProficiencyUuid,
                ProficiencyName = result.ProficiencyLevel?.ProficiencyName,
                YearsExperience = result.YearsExperience,
                IsPrimarySubject = result.IsPrimarySubject,
                Notes = result.Notes,
                CreatedAt = result.CreatedAt.DateTime,
                UpdatedAt = result.UpdatedAt?.DateTime
            }
        };
    }
}
