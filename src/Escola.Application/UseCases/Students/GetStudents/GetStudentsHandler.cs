namespace Escola.Application.UseCases.Students.GetStudents;

/// <summary>
/// Handler interface for getting all students.
/// </summary>
public interface IGetStudentsHandler
{
    /// <summary>
    /// Gets all students.
    /// </summary>
    /// <param name="request">Empty request (for future filtering/pagination)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of student DTOs</returns>
    Task<List<StudentResponse>> Handle(GetStudentsRequest request, CancellationToken cancellationToken = default);
}
