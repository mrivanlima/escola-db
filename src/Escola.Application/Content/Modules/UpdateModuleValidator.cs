using FluentValidation;

namespace Escola.Application.Content.Modules;

public class UpdateModuleValidator : AbstractValidator<UpdateModuleDto>
{
    public UpdateModuleValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.ModuleName) || 
                      !string.IsNullOrWhiteSpace(x.Description) ||
                      !string.IsNullOrWhiteSpace(x.ModuleType) ||
                      x.DifficultyLevel.HasValue ||
                      x.RecommendedAgeMin.HasValue ||
                      x.RecommendedAgeMax.HasValue ||
                      x.DisplayOrder.HasValue ||
                      !string.IsNullOrWhiteSpace(x.ThumbnailUrl) ||
                      !string.IsNullOrWhiteSpace(x.ModuleConfig) ||
                      x.IsPublished.HasValue)
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.ModuleName), () =>
        {
            RuleFor(x => x.ModuleName!)
                .MaximumLength(200)
                .WithMessage("ModuleName must not exceed 200 characters");
        });

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
    }
}
