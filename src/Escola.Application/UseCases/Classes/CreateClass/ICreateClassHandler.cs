namespace Escola.Application.UseCases.Classes.CreateClass;

/// <summary>
/// Handler interface for creating a class.
/// </summary>
public interface ICreateClassHandler
{
    /// <summary>
    /// Creates a new class.
    /// </summary>
    /// <param name="request">The class creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created class information.</returns>
    Task<CreateClassResponse> Handle(CreateClassRequest request, CancellationToken cancellationToken);
}
