using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherCertifications.GetTeacherCertification;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherCertifications;

public class GetTeacherCertificationHandler : IGetTeacherCertificationHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeacherCertificationHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeacherCertificationResponse> HandleAsync(GetTeacherCertificationRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacherCertification = await _context.TeacherCertifications
            .Include(tc => tc.Teacher).ThenInclude(t => t.User)
            .Include(tc => tc.Certification)
            .FirstOrDefaultAsync(tc => tc.Teacher.TeacherUuid == request.TeacherUuid && tc.Certification.CertificationUuid == request.CertificationUuid && tc.Teacher.TenantId == currentTenantId.Value && tc.Teacher.DeletedAt == null, cancellationToken)
            ?? throw new InvalidOperationException($"Teacher certification with TeacherUUID {request.TeacherUuid} and CertificationUUID {request.CertificationUuid} not found or access denied");

        return new GetTeacherCertificationResponse
        {
            TeacherCertification = new TeacherCertificationDto
            {
                TeacherUuid = teacherCertification.Teacher.TeacherUuid,
                TeacherName = teacherCertification.Teacher.User.FullName,
                CertificationUuid = teacherCertification.Certification.CertificationUuid,
                CertificationName = teacherCertification.Certification.CertificationName,
                ObtainedDate = teacherCertification.ObtainedDate,
                ExpiryDate = teacherCertification.ExpiryDate,
                CredentialNumber = teacherCertification.CredentialNumber,
                Notes = teacherCertification.Notes,
                CreatedAt = teacherCertification.CreatedAt.DateTime,
                UpdatedAt = teacherCertification.UpdatedAt?.DateTime
            }
        };
    }
}
