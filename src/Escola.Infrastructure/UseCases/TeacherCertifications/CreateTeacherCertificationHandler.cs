using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherCertifications.CreateTeacherCertification;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherCertifications;

public class CreateTeacherCertificationHandler : ICreateTeacherCertificationHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTeacherCertificationHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<CreateTeacherCertificationResponse> HandleAsync(CreateTeacherCertificationRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var teacherId = await _context.Teachers
            .Where(t => t.TeacherUuid == request.TeacherCertification.TeacherUuid && t.TenantId == currentTenantId.Value && t.DeletedAt == null)
            .Select(t => t.TeacherId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (teacherId == 0)
            throw new InvalidOperationException($"Teacher with UUID {request.TeacherCertification.TeacherUuid} not found or access denied");

        var certificationId = await _context.Certifications
            .Where(c => c.CertificationUuid == request.TeacherCertification.CertificationUuid && c.DeletedAt == null)
            .Select(c => c.CertificationId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (certificationId == 0)
            throw new InvalidOperationException($"Certification with UUID {request.TeacherCertification.CertificationUuid} not found");

        var teacherCertification = new TeacherCertification
        {
            TeacherId = teacherId,
            CertificationId = certificationId,
            ObtainedDate = request.TeacherCertification.ObtainedDate,
            ExpiryDate = request.TeacherCertification.ExpiryDate,
            CredentialNumber = request.TeacherCertification.CredentialNumber,
            Notes = request.TeacherCertification.Notes,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.GetCurrentUserId()
        };

        _context.TeacherCertifications.Add(teacherCertification);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.TeacherCertifications
            .Include(tc => tc.Teacher).ThenInclude(t => t.User)
            .Include(tc => tc.Certification)
            .FirstAsync(tc => tc.TeacherId == teacherId && tc.CertificationId == certificationId, cancellationToken);

        return new CreateTeacherCertificationResponse
        {
            TeacherCertification = new TeacherCertificationDto
            {
                TeacherUuid = result.Teacher.TeacherUuid,
                TeacherName = result.Teacher.User.FullName,
                CertificationUuid = result.Certification.CertificationUuid,
                CertificationName = result.Certification.CertificationName,
                ObtainedDate = result.ObtainedDate,
                ExpiryDate = result.ExpiryDate,
                CredentialNumber = result.CredentialNumber,
                Notes = result.Notes,
                CreatedAt = result.CreatedAt.DateTime,
                UpdatedAt = result.UpdatedAt?.DateTime
            }
        };
    }
}
