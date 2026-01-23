using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

/// <summary>
/// Validator for UpdateSchoolYearDto.
/// </summary>
public class UpdateSchoolYearValidator : AbstractValidator<UpdateSchoolYearDto>
{
    public UpdateSchoolYearValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("School year name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.EndDate > x.StartDate)
            .WithMessage("End date must be after start date when both are provided.");
    }
}
