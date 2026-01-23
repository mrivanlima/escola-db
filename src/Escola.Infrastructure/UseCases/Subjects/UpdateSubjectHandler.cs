using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Subjects.UpdateSubject;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Subjects;

public class UpdateSubjectHandler : IUpdateSubjectHandler
{
    private readonly EscolaDbContext _context;

    public UpdateSubjectHandler(EscolaDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSubjectResponse> HandleAsync(UpdateSubjectRequest request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects
            .Include(s => s.GradeLevel)
            .FirstOrDefaultAsync(s => s.SubjectUuid == request.SubjectUuid, cancellationToken)
            ?? throw new InvalidOperationException($"Subject with UUID {request.SubjectUuid} not found");

        subject.SubjectName = request.Subject.SubjectName;
        subject.Description = request.Subject.Description;
        
        if (request.Subject.GradeLevelUuid.HasValue)
        {
            var gradeLevelId = await _context.GradeLevels
                .Where(g => g.GradeLevelUuid == request.Subject.GradeLevelUuid.Value)
                .Select(g => g.GradeLevelId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (gradeLevelId == 0)
                throw new InvalidOperationException($"Grade level with UUID {request.Subject.GradeLevelUuid} not found");
            
            subject.GradeLevelId = gradeLevelId;
        }
        else
        {
            subject.GradeLevelId = null;
        }
        
        subject.IsActive = request.Subject.IsActive;
        subject.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(subject).Reference(s => s.GradeLevel).LoadAsync(cancellationToken);

        return new UpdateSubjectResponse
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
