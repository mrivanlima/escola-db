namespace Escola.Application.UseCases.Teachers.CreateTeacher;

/// <summary>
/// Handler interface for creating a teacher.
/// </summary>
public interface ICreateTeacherHandler
{
    /// <summary>
    /// Creates a new teacher.
    /// </summary>
    /// <param name="request">The teacher creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created teacher information.</returns>
    Task<CreateTeacherResponse> Handle(CreateTeacherRequest request, CancellationToken cancellationToken);
}
