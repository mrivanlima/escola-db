using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ClassStudents.GetClassStudent;

public class GetClassStudentRequest
{
    public Guid ClassUuid { get; set; }
    public Guid StudentUuid { get; set; }
}

public class GetClassStudentResponse
{
    public ClassStudentDto ClassStudent { get; set; } = null!;
}

public interface IGetClassStudentHandler
{
    Task<GetClassStudentResponse> HandleAsync(GetClassStudentRequest request, CancellationToken cancellationToken = default);
}
