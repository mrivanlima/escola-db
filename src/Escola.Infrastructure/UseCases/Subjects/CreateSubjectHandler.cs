using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Subjects.CreateSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Subjects;

public class CreateSubjectHandler : ICreateSubjectHandler
{
    private readonly EscolaDbContext _context;

    public CreateSubjectHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateSubjectResponse> HandleAsync(CreateSubjectRequest request, CancellationToken cancellationToken)
    {
        short? gradeLevelId = null;
        if (request.Subject.GradeLevelUuid.HasValue)
        {
            gradeLevelId = await _context.GradeLevels
                .Where(g => g.GradeLevelUuid == request.Subject.GradeLevelUuid.Value)
                .Select(g => g.GradeLevelId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (!gradeLevelId.HasValue)
                throw new InvalidOperationException($"Grade level with UUID {request.Subject.GradeLevelUuid} not found");
        }

        var subject = new Subject
        {
            SubjectUuid = Guid.NewGuid(),
            SubjectName = request.Subject.SubjectName,
            Description = request.Subject.Description,
            GradeLevelId = gradeLevelId,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.Subjects
            .Include(s => s.GradeLevel)
            .FirstAsync(s => s.SubjectId == subject.SubjectId, cancellationToken);

        return new CreateSubjectResponse
        {
            Subject = new SubjectDto
            {
                SubjectUuid = result.SubjectUuid,
                SubjectName = result.SubjectName,
                Description = result.Description,
                GradeLevelUuid = result.GradeLevel?.GradeLevelUuid,
                GradeLevelName = result.GradeLevel?.Name,
                IsActive = result.IsActive,
                CreatedAt = result.CreatedAt.DateTime,
                UpdatedAt = result.UpdatedAt?.DateTime
            }
        };
    }
}
