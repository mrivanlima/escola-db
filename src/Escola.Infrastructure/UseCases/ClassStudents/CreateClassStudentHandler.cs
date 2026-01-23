using Escola.Application.DTOs.School;
using Escola.Application.UseCases.ClassStudents.CreateClassStudent;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.ClassStudents;

public class CreateClassStudentHandler : ICreateClassStudentHandler
{
    private readonly EscolaDbContext _context;

    public CreateClassStudentHandler(EscolaDbContext context) => _context = context;

    public async Task<CreateClassStudentResponse> HandleAsync(CreateClassStudentRequest request, CancellationToken cancellationToken = default)
    {
        var dto = request.ClassStudent;

        var classEntity = await _context.Set<Class>()
            .FirstOrDefaultAsync(c => c.ClassUuid == dto.ClassUuid && c.DeletedAt == null, cancellationToken);
        if (classEntity == null) throw new InvalidOperationException("Class not found.");

        var student = await _context.Set<Student>()
            .FirstOrDefaultAsync(s => s.StudentUuid == dto.StudentUuid && s.DeletedAt == null, cancellationToken);
        if (student == null) throw new InvalidOperationException("Student not found.");

        var status = await _context.Set<EnrollmentStatus>()
            .FirstOrDefaultAsync(es => es.EnrollmentStatusUuid == dto.StatusUuid, cancellationToken);
        if (status == null) throw new InvalidOperationException("Enrollment status not found.");

        var classStudent = new ClassStudent
        {
            ClassId = classEntity.ClassId,
            StudentId = student.StudentId,
            TenantId = classEntity.TenantId,
            EnrollmentDate = dto.EnrollmentDate,
            StatusId = status.EnrollmentStatusId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Set<ClassStudent>().Add(classStudent);
        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(classStudent).Reference(cs => cs.Class).LoadAsync(cancellationToken);
        await _context.Entry(classStudent).Reference(cs => cs.Student).LoadAsync(cancellationToken);
        await _context.Entry(classStudent).Reference(cs => cs.Status).LoadAsync(cancellationToken);

        return new CreateClassStudentResponse
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
