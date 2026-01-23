using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ClassStudents.GetClassStudent;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ClassStudents;

public class GetClassStudentHandler : IGetClassStudentHandler
{
    private readonly EscolaDbContext _context;

    public GetClassStudentHandler(EscolaDbContext context) => _context = context;

    public async Task<GetClassStudentResponse> HandleAsync(GetClassStudentRequest request, CancellationToken cancellationToken = default)
    {
        var classStudent = await _context.Set<ClassStudent>()
            .Include(cs => cs.Class)
            .Include(cs => cs.Student)
            .Include(cs => cs.Status)
            .FirstOrDefaultAsync(cs => cs.Class.ClassUuid == request.ClassUuid 
                && cs.Student.StudentUuid == request.StudentUuid 
                && cs.DeletedAt == null, cancellationToken);

        if (classStudent == null) throw new InvalidOperationException("Class student enrollment not found.");

        return new GetClassStudentResponse
        {
            ClassStudent = new ClassStudentDto
            {
                ClassUuid = classStudent.Class.ClassUuid,
                StudentUuid = classStudent.Student.StudentUuid,
                ClassName = classStudent.Class.ClassName,
                StudentName = $"{classStudent.Student.FirstName} {classStudent.Student.LastName}",
                EnrollmentDate = classStudent.EnrollmentDate,
                StatusUuid = classStudent.Status.EnrollmentStatusUuid,
                StatusName = classStudent.Status.Name,
                CreatedAt = classStudent.CreatedAt.DateTime,
                UpdatedAt = classStudent.UpdatedAt?.DateTime
            }
        };
    }
}
