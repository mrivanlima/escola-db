namespace Escola.Application.UseCases.Teachers.GetTeachers;

/// <summary>
/// Handler interface for retrieving all teachers.
/// </summary>
public interface IGetTeachersHandler
{
    /// <summary>
    /// Retrieves all teachers from the system.
    /// </summary>
    /// <param name="request">The request containing query parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of teachers.</returns>
    Task<List<TeacherResponse>> Handle(GetTeachersRequest request, CancellationToken cancellationToken);
}
