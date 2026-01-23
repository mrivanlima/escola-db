using FluentValidation;

namespace Escola.Application.Content.ActivityResources;

public class CreateActivityResourceValidator : AbstractValidator<CreateActivityResourceDto>
{
    public CreateActivityResourceValidator()
    {
        RuleFor(x => x.ActivityUuid)
            .NotEmpty()
            .WithMessage("ActivityUuid is required");

        RuleFor(x => x.ResourceName)
            .NotEmpty()
            .WithMessage("ResourceName is required")
            .MaximumLength(200)
            .WithMessage("ResourceName must not exceed 200 characters");

        RuleFor(x => x.ResourceType)
            .NotEmpty()
            .WithMessage("ResourceType is required")
            .MaximumLength(50)
            .WithMessage("ResourceType must not exceed 50 characters");

        RuleFor(x => x.MediaFileUuid)
            .NotEmpty()
            .WithMessage("MediaFileUuid is required");

        When(x => !string.IsNullOrWhiteSpace(x.UsageContext), () =>
        {
            RuleFor(x => x.UsageContext!)
                .MaximumLength(500)
                .WithMessage("UsageContext must not exceed 500 characters");
        });
    }
}

public class UpdateActivityResourceValidator : AbstractValidator<UpdateActivityResourceDto>
{
    public UpdateActivityResourceValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.ResourceName) ||
                      !string.IsNullOrWhiteSpace(x.ResourceType) ||
                      x.DisplayOrder.HasValue ||
                      x.IsRequired.HasValue ||
                      !string.IsNullOrWhiteSpace(x.UsageContext) ||
                      !string.IsNullOrWhiteSpace(x.ResourceConfig) ||
                      x.IsPublished.HasValue)
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.ResourceName), () =>
        {
            RuleFor(x => x.ResourceName!)
                .MaximumLength(200)
                .WithMessage("ResourceName must not exceed 200 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.ResourceType), () =>
        {
            RuleFor(x => x.ResourceType!)
                .MaximumLength(50)
                .WithMessage("ResourceType must not exceed 50 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.UsageContext), () =>
        {
            RuleFor(x => x.UsageContext!)
                .MaximumLength(500)
                .WithMessage("UsageContext must not exceed 500 characters");
        });
    }
}
