using FluentValidation;

namespace Escola.Application.Content.Activities;

public class UpdateActivityValidator : AbstractValidator<UpdateActivityDto>
{
    public UpdateActivityValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.ActivityName) ||
                      !string.IsNullOrWhiteSpace(x.Description) ||
                      !string.IsNullOrWhiteSpace(x.ActivityType) ||
                      x.DisplayOrder.HasValue ||
                      x.EstimatedDuration.HasValue ||
                      x.PointsReward.HasValue ||
                      !string.IsNullOrWhiteSpace(x.ActivityData) ||
                      !string.IsNullOrWhiteSpace(x.ThumbnailUrl) ||
                      x.IsPublished.HasValue)
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.ActivityName), () =>
        {
            RuleFor(x => x.ActivityName!)
                .MaximumLength(200)
                .WithMessage("ActivityName must not exceed 200 characters");
        });

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
