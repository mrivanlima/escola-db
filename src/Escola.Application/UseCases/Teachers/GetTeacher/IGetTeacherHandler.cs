namespace Escola.Application.UseCases.Teachers.GetTeacher;

/// <summary>
/// Handler interface for retrieving a single teacher by UUID.
/// </summary>
public interface IGetTeacherHandler
{
    /// <summary>
    /// Retrieves a teacher by UUID.
    /// </summary>
    /// <param name="teacherUuid">The teacher UUID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Teacher information or null if not found.</returns>
    Task<GetTeachers.TeacherResponse?> Handle(Guid teacherUuid, CancellationToken cancellationToken);
}
