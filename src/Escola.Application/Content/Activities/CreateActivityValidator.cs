using FluentValidation;

namespace Escola.Application.Content.Activities;

public class CreateActivityValidator : AbstractValidator<CreateActivityDto>
{
    public CreateActivityValidator()
    {
        RuleFor(x => x.ModuleUuid)
            .NotEmpty()
            .WithMessage("ModuleUuid is required");

        RuleFor(x => x.ActivityName)
            .NotEmpty()
            .WithMessage("ActivityName is required")
            .MaximumLength(200)
            .WithMessage("ActivityName must not exceed 200 characters");

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(2000)
                .WithMessage("Description must not exceed 2000 characters");
        });

        When(x => x.EstimatedDuration.HasValue, () =>
        {
            RuleFor(x => x.EstimatedDuration!.Value)
                .GreaterThan(0)
                .WithMessage("EstimatedDuration must be greater than 0");
        });

        When(x => x.PointsReward.HasValue, () =>
        {
            RuleFor(x => x.PointsReward!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PointsReward must be 0 or greater");
        });
    }
}
