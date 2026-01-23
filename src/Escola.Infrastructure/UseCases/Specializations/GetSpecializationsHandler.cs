using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Specializations.GetSpecializations;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Specializations;

public class GetSpecializationsHandler : IGetSpecializationsHandler
{
    private readonly EscolaDbContext _context;

    public GetSpecializationsHandler(EscolaDbContext context) => _context = context;

    public async Task<GetSpecializationsResponse> HandleAsync(GetSpecializationsRequest request, CancellationToken cancellationToken = default)
    {
        var specializations = await _context.Set<Specialization>()
            .Where(s => s.DeletedAt == null)
            .OrderBy(s => s.SpecializationName)
            .ToListAsync(cancellationToken);

        return new GetSpecializationsResponse
        {
            Specializations = specializations.Select(s => new SpecializationDto
            {
                SpecializationUuid = s.SpecializationUuid,
                SpecializationName = s.SpecializationName,
                Description = s.Description,
                IsActive = s.IsActive
            }).ToList()
        };
    }
}
