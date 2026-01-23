namespace Escola.Application.UseCases.Teachers.UpdateTeacher;

/// <summary>
/// Handler interface for updating a teacher.
/// </summary>
public interface IUpdateTeacherHandler
{
    /// <summary>
    /// Updates an existing teacher.
    /// </summary>
    /// <param name="teacherUuid">The teacher UUID.</param>
    /// <param name="request">The teacher update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated teacher information or null if not found.</returns>
    Task<UpdateTeacherResponse?> Handle(Guid teacherUuid, UpdateTeacherRequest request, CancellationToken cancellationToken);
}
