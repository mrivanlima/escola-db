using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.Teachers.GetTeachers;

public class GetTeachersRequest
{
    public Guid? TenantUuid { get; set; }
    public Guid? SpecializationUuid { get; set; }
}

public class GetTeachersResponse
{
    public List<TeacherDto> Teachers { get; set; } = new();
}

public interface IGetTeachersHandler
{
    Task<GetTeachersResponse> HandleAsync(GetTeachersRequest request, CancellationToken cancellationToken = default);
}
