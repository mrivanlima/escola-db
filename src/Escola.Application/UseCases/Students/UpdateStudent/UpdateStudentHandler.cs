namespace Escola.Application.UseCases.Students.UpdateStudent;

/// <summary>
/// Handler interface for updating a student.
/// </summary>
public interface IUpdateStudentHandler
{
    /// <summary>
    /// Updates a student.
    /// </summary>
    /// <param name="studentUuid">Student UUID</param>
    /// <param name="request">Update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated student response or null if not found</returns>
    Task<UpdateStudentResponse?> Handle(Guid studentUuid, UpdateStudentRequest request, CancellationToken cancellationToken = default);
}
