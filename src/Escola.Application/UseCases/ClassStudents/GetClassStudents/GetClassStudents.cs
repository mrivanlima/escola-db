using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ClassStudents.GetClassStudents;

public class GetClassStudentsRequest
{
    public Guid? ClassUuid { get; set; }
    public Guid? StudentUuid { get; set; }
}

public class GetClassStudentsResponse
{
    public List<ClassStudentDto> ClassStudents { get; set; } = new();
}

public interface IGetClassStudentsHandler
{
    Task<GetClassStudentsResponse> HandleAsync(GetClassStudentsRequest request, CancellationToken cancellationToken = default);
}
