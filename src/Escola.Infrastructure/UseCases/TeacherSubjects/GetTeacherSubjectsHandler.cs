using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherSubjects.GetTeacherSubjects;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherSubjects;

public class GetTeacherSubjectsHandler : IGetTeacherSubjectsHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeacherSubjectsHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeacherSubjectsResponse> HandleAsync(GetTeacherSubjectsRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.TeacherSubjects
            .Include(ts => ts.Teacher).ThenInclude(t => t.User)
            .Include(ts => ts.Subject)
            .Include(ts => ts.ProficiencyLevel)
            .Where(ts => ts.Teacher.TenantId == currentTenantId.Value && ts.Teacher.DeletedAt == null)
            .AsQueryable();

        if (request.TeacherUuid.HasValue)
            query = query.Where(ts => ts.Teacher.TeacherUuid == request.TeacherUuid.Value);

        if (request.SubjectUuid.HasValue)
            query = query.Where(ts => ts.Subject.SubjectUuid == request.SubjectUuid.Value);

        var teacherSubjects = await query
            .OrderBy(ts => ts.Teacher.User.FullName)
            .ThenBy(ts => ts.Subject.SubjectName)
            .ToListAsync(cancellationToken);

        return new GetTeacherSubjectsResponse
        {
            TeacherSubjects = teacherSubjects.Select(ts => new TeacherSubjectDto
            {
                TeacherUuid = ts.Teacher.TeacherUuid,
                TeacherName = ts.Teacher.User.FullName,
                SubjectUuid = ts.Subject.SubjectUuid,
                SubjectName = ts.Subject.SubjectName,
                ProficiencyUuid = ts.ProficiencyLevel?.ProficiencyUuid,
                ProficiencyName = ts.ProficiencyLevel?.ProficiencyName,
                YearsExperience = ts.YearsExperience,
                IsPrimarySubject = ts.IsPrimarySubject,
                Notes = ts.Notes,
                CreatedAt = ts.CreatedAt.DateTime,
                UpdatedAt = ts.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
