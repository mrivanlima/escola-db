using Escola.Application.UseCases.Students.GetStudents;

namespace Escola.Application.UseCases.Students.GetStudent;

/// <summary>
/// Handler interface for getting a single student by UUID.
/// </summary>
public interface IGetStudentHandler
{
    /// <summary>
    /// Gets a student by UUID.
    /// </summary>
    /// <param name="studentUuid">Student UUID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student DTO or null if not found</returns>
    Task<StudentResponse?> Handle(Guid studentUuid, CancellationToken cancellationToken = default);
}
