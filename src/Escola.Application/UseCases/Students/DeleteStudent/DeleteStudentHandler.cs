namespace Escola.Application.UseCases.Students.DeleteStudent;

/// <summary>
/// Handler interface for soft-deleting a student.
/// </summary>
public interface IDeleteStudentHandler
{
    /// <summary>
    /// Soft-deletes a student by UUID.
    /// </summary>
    /// <param name="studentUuid">The UUID of the student to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if deleted successfully, false if not found.</returns>
    Task<bool> Handle(Guid studentUuid, CancellationToken cancellationToken = default);
}
