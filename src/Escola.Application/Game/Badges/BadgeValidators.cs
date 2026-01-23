using FluentValidation;

namespace Escola.Application.Game.Badges;

public class CreateBadgeValidator : AbstractValidator<CreateBadgeDto>
{
    public CreateBadgeValidator()
    {
        RuleFor(x => x.BadgeName)
            .NotEmpty()
            .WithMessage("BadgeName is required")
            .MaximumLength(100)
            .WithMessage("BadgeName must not exceed 100 characters");

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
        });

        When(x => x.PointsRequired.HasValue, () =>
        {
            RuleFor(x => x.PointsRequired!.Value)
                .GreaterThan(0)
                .WithMessage("PointsRequired must be greater than 0");
        });
    }
}

public class UpdateBadgeValidator : AbstractValidator<UpdateBadgeDto>
{
    public UpdateBadgeValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.BadgeName) ||
                      !string.IsNullOrWhiteSpace(x.Description) ||
                      !string.IsNullOrWhiteSpace(x.BadgeType) ||
                      !string.IsNullOrWhiteSpace(x.IconUrl) ||
                      !string.IsNullOrWhiteSpace(x.Rarity) ||
                      x.PointsRequired.HasValue ||
                      !string.IsNullOrWhiteSpace(x.Criteria) ||
                      x.DisplayOrder.HasValue ||
                      x.IsActive.HasValue)
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.BadgeName), () =>
        {
            RuleFor(x => x.BadgeName!)
                .MaximumLength(100)
                .WithMessage("BadgeName must not exceed 100 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
        });

        When(x => x.PointsRequired.HasValue, () =>
        {
            RuleFor(x => x.PointsRequired!.Value)
                .GreaterThan(0)
                .WithMessage("PointsRequired must be greater than 0");
        });
    }
}
