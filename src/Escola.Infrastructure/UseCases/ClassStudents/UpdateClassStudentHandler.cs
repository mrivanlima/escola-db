using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ClassStudents.UpdateClassStudent;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ClassStudents;

public class UpdateClassStudentHandler : IUpdateClassStudentHandler
{
    private readonly EscolaDbContext _context;

    public UpdateClassStudentHandler(EscolaDbContext context) => _context = context;

    public async Task<UpdateClassStudentResponse> HandleAsync(UpdateClassStudentRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.ClassStudent;

        var classStudent = await _context.Set<ClassStudent>()
            .Include(cs => cs.Class)
            .Include(cs => cs.Student)
            .Include(cs => cs.Status)
            .FirstOrDefaultAsync(cs => cs.Class.ClassUuid == request.ClassUuid 
                && cs.Student.StudentUuid == request.StudentUuid 
                && cs.DeletedAt == null, cancellationToken);

        if (classStudent == null) throw new InvalidOperationException("Class student enrollment not found.");

        if (dto.EnrollmentDate.HasValue)
            classStudent.EnrollmentDate = dto.EnrollmentDate.Value;

        if (dto.StatusUuid.HasValue)
        {
            var status = await _context.Set<EnrollmentStatus>()
                .FirstOrDefaultAsync(es => es.EnrollmentStatusUuid == dto.StatusUuid.Value, cancellationToken);
            if (status == null) throw new InvalidOperationException("Enrollment status not found.");
            classStudent.StatusId = status.EnrollmentStatusId;
            await _context.Entry(classStudent).Reference(cs => cs.Status).LoadAsync(cancellationToken);
        }

        classStudent.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateClassStudentResponse
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
