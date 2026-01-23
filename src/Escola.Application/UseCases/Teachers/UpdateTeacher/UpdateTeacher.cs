using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Teachers.UpdateTeacher;

public class UpdateTeacherRequest
{
    public Guid TeacherUuid { get; set; }
    public UpdateTeacherDto Teacher { get; set; } = null!;
}

public class UpdateTeacherResponse
{
    public TeacherDto Teacher { get; set; } = null!;
}

public interface IUpdateTeacherHandler
{
    Task<UpdateTeacherResponse> HandleAsync(UpdateTeacherRequest request, CancellationToken cancellationToken = default);
}
