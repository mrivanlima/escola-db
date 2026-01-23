using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Subjects.GetSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Subjects;

public class GetSubjectHandler : IGetSubjectHandler
{
    private readonly EscolaDbContext _context;

    public GetSubjectHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<GetSubjectResponse> HandleAsync(GetSubjectRequest request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects
            .Include(s => s.GradeLevel)
            .FirstOrDefaultAsync(s => s.SubjectUuid == request.SubjectUuid, cancellationToken)
            ?? throw new InvalidOperationException($"Subject with UUID {request.SubjectUuid} not found");

        return new GetSubjectResponse
        {
            Subject = new SubjectDto
            {
                SubjectUuid = subject.SubjectUuid,
                SubjectName = subject.SubjectName,
                Description = subject.Description,
                GradeLevelUuid = subject.GradeLevel?.GradeLevelUuid,
                GradeLevelName = subject.GradeLevel?.Name,
                IsActive = subject.IsActive,
                CreatedAt = subject.CreatedAt.DateTime,
                UpdatedAt = subject.UpdatedAt?.DateTime
            }
        };
    }
}
