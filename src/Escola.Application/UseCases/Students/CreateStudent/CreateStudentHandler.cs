namespace Escola.Application.UseCases.Students.CreateStudent;

/// <summary>
/// Handler interface for creating a new student.
/// Implementation will be in Infrastructure layer to respect Clean Architecture.
/// </summary>
public interface ICreateStudentHandler
{
    Task<CreateStudentResponse> Handle(CreateStudentRequest request, CancellationToken cancellationToken = default);
}
