using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Subjects.GetSubjects;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Subjects;

public class GetSubjectsHandler : IGetSubjectsHandler
{
    private readonly EscolaDbContext _context;

    public GetSubjectsHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetSubjectsResponse> HandleAsync(GetSubjectsRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Subjects
            .Include(s => s.GradeLevel)
            .AsQueryable();

        if (request.GradeLevelUuid.HasValue)
            query = query.Where(s => s.GradeLevel != null && s.GradeLevel.GradeLevelUuid == request.GradeLevelUuid.Value);

        var subjects = await query
            .OrderBy(s => s.SubjectName)
            .ToListAsync(cancellationToken);

        return new GetSubjectsResponse
        {
            Subjects = subjects.Select(s => new SubjectDto
            {
                SubjectUuid = s.SubjectUuid,
                SubjectName = s.SubjectName,
                Description = s.Description,
                GradeLevelUuid = s.GradeLevel?.GradeLevelUuid,
                GradeLevelName = s.GradeLevel?.Name,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt.DateTime,
                UpdatedAt = s.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
