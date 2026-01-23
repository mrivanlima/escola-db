using Escola.Application.DTOs.School;

namespace Escola.Application.UseCases.StudentGuardians.GetStudentGuardians;

public class GetStudentGuardiansRequest
{
    public Guid? StudentUuid { get; set; }
    public Guid? GuardianUuid { get; set; }
    public Guid? TenantUuid { get; set; }
}

public class GetStudentGuardiansResponse
{
    public List<StudentGuardianDto> StudentGuardians { get; set; } = new();
}

public interface IGetStudentGuardiansHandler
{
    Task<GetStudentGuardiansResponse> HandleAsync(
        GetStudentGuardiansRequest request,
        CancellationToken cancellationToken = default);
}
