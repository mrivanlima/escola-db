using Escola.Application.DTOs.School;
using Escola.Application.Services;
using Escola.Application.UseCases.TeacherCertifications.GetTeacherCertifications;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.TeacherCertifications;

public class GetTeacherCertificationsHandler : IGetTeacherCertificationsHandler
{
    private readonly EscolaDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTeacherCertificationsHandler(EscolaDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTeacherCertificationsResponse> HandleAsync(GetTeacherCertificationsRequest request, CancellationToken cancellationToken)
    {
        // Get current tenant ID from context
        var currentTenantId = _currentUserService.GetCurrentTenantId();
        if (!currentTenantId.HasValue)
            throw new UnauthorizedAccessException("No tenant context available");

        var query = _context.TeacherCertifications
            .Include(tc => tc.Teacher).ThenInclude(t => t.User)
            .Include(tc => tc.Certification)
            .Where(tc => tc.Teacher.TenantId == currentTenantId.Value && tc.Teacher.DeletedAt == null)
            .AsQueryable();

        if (request.TeacherUuid.HasValue)
            query = query.Where(tc => tc.Teacher.TeacherUuid == request.TeacherUuid.Value);

        if (request.CertificationUuid.HasValue)
            query = query.Where(tc => tc.Certification.CertificationUuid == request.CertificationUuid.Value);

        var teacherCertifications = await query
            .OrderBy(tc => tc.Teacher.User.FullName)
            .ThenBy(tc => tc.Certification.CertificationName)
            .ToListAsync(cancellationToken);

        return new GetTeacherCertificationsResponse
        {
            TeacherCertifications = teacherCertifications.Select(tc => new TeacherCertificationDto
            {
                TeacherUuid = tc.Teacher.TeacherUuid,
                TeacherName = tc.Teacher.User.FullName,
                CertificationUuid = tc.Certification.CertificationUuid,
                CertificationName = tc.Certification.CertificationName,
                ObtainedDate = tc.ObtainedDate,
                ExpiryDate = tc.ExpiryDate,
                CredentialNumber = tc.CredentialNumber,
                Notes = tc.Notes,
                CreatedAt = tc.CreatedAt.DateTime,
                UpdatedAt = tc.UpdatedAt?.DateTime
            }).ToList()
        };
    }
}
