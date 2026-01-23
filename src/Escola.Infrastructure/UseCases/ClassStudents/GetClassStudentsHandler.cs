using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ClassStudents.GetClassStudents;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ClassStudents;

public class GetClassStudentsHandler : IGetClassStudentsHandler
{
    private readonly EscolaDbContext _context;

    public GetClassStudentsHandler(EscolaDbContext context) => _context = context;

    public async Task<GetClassStudentsResponse> HandleAsync(GetClassStudentsRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<ClassStudent>()
            .Include(cs => cs.Class)
            .Include(cs => cs.Student)
            .Include(cs => cs.Status)
            .Where(cs => cs.DeletedAt == null);

        if (request.ClassUuid.HasValue)
            query = query.Where(cs => cs.Class.ClassUuid == request.ClassUuid.Value);

        if (request.StudentUuid.HasValue)
            query = query.Where(cs => cs.Student.StudentUuid == request.StudentUuid.Value);

        var classStudents = await query
            .OrderBy(cs => cs.Class.ClassName)
            .ThenBy(cs => cs.Student.FirstName)
            .ToListAsync(cancellationToken);

        return new GetClassStudentsResponse
        {
            ClassStudents = classStudents.Select(cs => new ClassStudentDto
            {
                ClassUuid = cs.Class.ClassUuid,
                StudentUuid = cs.Student.StudentUuid,
                ClassName = cs.Class.ClassName,
                StudentName = $"{cs.Student.FirstName} {cs.Student.LastName}",
                EnrollmentDate = cs.EnrollmentDate,
                StatusUuid = cs.Status.EnrollmentStatusUuid,
                StatusName = cs.Status.Name,
                CreatedAt = cs.CreatedAt.DateTime,
                UpdatedAt = cs.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
