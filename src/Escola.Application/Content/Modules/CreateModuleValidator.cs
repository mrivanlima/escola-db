using FluentValidation;

namespace Escola.Application.Content.Modules;

public class CreateModuleValidator : AbstractValidator<CreateModuleDto>
{
    public CreateModuleValidator()
    {
        RuleFor(x => x.ModuleName)
            .NotEmpty()
            .WithMessage("ModuleName is required")
            .MaximumLength(200)
            .WithMessage("ModuleName must not exceed 200 characters");

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(2000)
                .WithMessage("Description must not exceed 2000 characters");
        });

        When(x => x.DifficultyLevel.HasValue, () =>
        {
            RuleFor(x => x.DifficultyLevel!.Value)
                .InclusiveBetween(1, 5)
                .WithMessage("DifficultyLevel must be between 1 and 5");
        });

        When(x => x.RecommendedAgeMin.HasValue, () =>
        {
            RuleFor(x => x.RecommendedAgeMin!.Value)
                .GreaterThan(0)
                .WithMessage("RecommendedAgeMin must be greater than 0");
        });

        When(x => x.RecommendedAgeMax.HasValue, () =>
        {
            RuleFor(x => x.RecommendedAgeMax!.Value)
                .GreaterThan(0)
                .WithMessage("RecommendedAgeMax must be greater than 0");
        });

        When(x => x.RecommendedAgeMin.HasValue && x.RecommendedAgeMax.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.RecommendedAgeMax >= x.RecommendedAgeMin)
                .WithMessage("RecommendedAgeMax must be greater than or equal to RecommendedAgeMin");
        });
    }
}
