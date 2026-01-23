using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Teachers.CreateTeacher;

public class CreateTeacherRequest
{
    public CreateTeacherDto Teacher { get; set; } = null!;
}

public class CreateTeacherResponse
{
    public TeacherDto Teacher { get; set; } = null!;
}

public interface ICreateTeacherHandler
{
    Task<CreateTeacherResponse> HandleAsync(CreateTeacherRequest request, CancellationToken cancellationToken = default);
}
