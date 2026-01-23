using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.StudentGuardians.GetStudentGuardian;

public class GetStudentGuardianRequest
{
    public Guid StudentUuid { get; set; }
    public Guid GuardianUuid { get; set; }
}

public class GetStudentGuardianResponse
{
    public StudentGuardianDto StudentGuardian { get; set; } = null!;
}

public interface IGetStudentGuardianHandler
{
    Task<GetStudentGuardianResponse> HandleAsync(
        GetStudentGuardianRequest request,
        CancellationToken cancellationToken = default);
}
