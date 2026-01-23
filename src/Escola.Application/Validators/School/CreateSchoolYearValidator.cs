using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

/// <summary>
/// Validator for CreateSchoolYearDto.
/// </summary>
public class CreateSchoolYearValidator : AbstractValidator<CreateSchoolYearDto>
{
    public CreateSchoolYearValidator()
    {
        RuleFor(x => x.TenantUuid)
            .NotEmpty()
            .WithMessage("Tenant UUID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("School year name is required.")
            .MaximumLength(100)
            .WithMessage("School year name cannot exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");
    }
}
