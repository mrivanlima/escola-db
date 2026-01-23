using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.ClassStudents.UpdateClassStudent;

public class UpdateClassStudentRequest
{
    public Guid ClassUuid { get; set; }
    public Guid StudentUuid { get; set; }
    public UpdateClassStudentDto ClassStudent { get; set; } = null!;
}

public class UpdateClassStudentResponse
{
    public ClassStudentDto ClassStudent { get; set; } = null!;
}

public interface IUpdateClassStudentHandler
{
    Task<UpdateClassStudentResponse> HandleAsync(UpdateClassStudentRequest request, CancellationToken cancellationToken = default);
}
