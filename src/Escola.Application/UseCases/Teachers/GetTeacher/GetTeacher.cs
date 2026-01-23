using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Teachers.GetTeacher;

public class GetTeacherRequest
{
    public Guid TeacherUuid { get; set; }
}

public class GetTeacherResponse
{
    public TeacherDto Teacher { get; set; } = null!;
}

public interface IGetTeacherHandler
{
    Task<GetTeacherResponse> HandleAsync(GetTeacherRequest request, CancellationToken cancellationToken = default);
}
