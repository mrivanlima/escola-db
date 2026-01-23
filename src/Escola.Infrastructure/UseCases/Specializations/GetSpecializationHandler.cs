using Escola.Application.DTOs.School;
using Escola.Application.UseCases.Specializations.GetSpecialization;
using Escola.Domain.School;
using Escola.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.UseCases.Specializations;

public class GetSpecializationHandler : IGetSpecializationHandler
{
    private readonly EscolaDbContext _context;

    public GetSpecializationHandler(EscolaDbContext context) => _context = context;

    public async Task<GetSpecializationResponse> HandleAsync(GetSpecializationRequest request, CancellationToken cancellationToken = default)
    {
        var specialization = await _context.Set<Specialization>()
            .FirstOrDefaultAsync(s => s.SpecializationUuid == request.SpecializationUuid && s.DeletedAt == null, cancellationToken);

        if (specialization == null) throw new InvalidOperationException("Specialization not found.");

        return new GetSpecializationResponse
        {
            Specialization = new SpecializationDto
            {
                SpecializationUuid = specialization.SpecializationUuid,
                SpecializationName = specialization.SpecializationName,
                Description = specialization.Description,
                IsActive = specialization.IsActive
            }
        };
    }
}
