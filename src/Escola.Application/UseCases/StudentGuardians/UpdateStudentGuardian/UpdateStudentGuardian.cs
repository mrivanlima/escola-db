using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.StudentGuardians.UpdateStudentGuardian;

public class UpdateStudentGuardianRequest
{
    public Guid StudentUuid { get; set; }
    public Guid GuardianUuid { get; set; }
    public UpdateStudentGuardianDto StudentGuardian { get; set; } = null!;
}

public class UpdateStudentGuardianResponse
{
    public StudentGuardianDto StudentGuardian { get; set; } = null!;
}

public interface IUpdateStudentGuardianHandler
{
    Task<UpdateStudentGuardianResponse> HandleAsync(
        UpdateStudentGuardianRequest request,
        CancellationToken cancellationToken = default);
}
