namespace Escola.Application.UseCases.Teachers.DeleteTeacher;

/// <summary>
/// Handler interface for soft deleting a teacher.
/// </summary>
public interface IDeleteTeacherHandler
{
    /// <summary>
    /// Soft deletes a teacher by UUID.
    /// </summary>
    /// <param name="teacherUuid">The teacher UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if deleted, false if not found.</returns>
    Task<bool> Handle(Guid teacherUuid, CancellationToken cancellationToken);
}
