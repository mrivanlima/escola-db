using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ClassStudents.CreateClassStudent;

public class CreateClassStudentRequest
{
    public CreateClassStudentDto ClassStudent { get; set; } = null!;
}

public class CreateClassStudentResponse
{
    public ClassStudentDto ClassStudent { get; set; } = null!;
}

public interface ICreateClassStudentHandler
{
    Task<CreateClassStudentResponse> HandleAsync(CreateClassStudentRequest request, CancellationToken cancellationToken = default);
}
