using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherSubjects.GetTeacherSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherSubjects;

public class GetTeacherSubjectHandler : IGetTeacherSubjectHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeacherSubjectHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeacherSubjectResponse> HandleAsync(GetTeacherSubjectRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacherSubject = await _context.TeacherSubjects
            .Include(ts => ts.Teacher).ThenInclude(t => t.User)
            .Include(ts => ts.Subject)
            .Include(ts => ts.ProficiencyLevel)
            .FirstOrDefaultAsync(ts => ts.Teacher.TeacherUuid == request.TeacherUuid && ts.Subject.SubjectUuid == request.SubjectUuid && ts.Teacher.TenantId == currentTenantId.Value && ts.Teacher.DeletedAt == null, cancellationToken)
            ?? throw new InvalidOperationException($"Teacher subject with TeacherUUID {request.TeacherUuid} and SubjectUUID {request.SubjectUuid} not found or access denied");

        return new GetTeacherSubjectResponse
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
