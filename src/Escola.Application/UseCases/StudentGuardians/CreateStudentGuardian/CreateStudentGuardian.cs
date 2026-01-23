using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.StudentGuardians.CreateStudentGuardian;

public class CreateStudentGuardianRequest
{
    public CreateStudentGuardianDto StudentGuardian { get; set; } = null!;
}

public class CreateStudentGuardianResponse
{
    public StudentGuardianDto StudentGuardian { get; set; } = null!;
}

public interface ICreateStudentGuardianHandler
{
    Task<CreateStudentGuardianResponse> HandleAsync(
        CreateStudentGuardianRequest request,
        CancellationToken cancellationToken = default);
}
