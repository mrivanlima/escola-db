using FluentValidation;

namespace Escola.Application.Assets.MediaFiles;

public class CreateMediaFileValidator : AbstractValidator<CreateMediaFileDto>
{
    public CreateMediaFileValidator()
    {
        RuleFor(x => x.MimeTypeUuid)
            .NotEmpty()
            .WithMessage("MimeTypeUuid is required");

        RuleFor(x => x.OriginalName)
            .NotEmpty()
            .WithMessage("OriginalName is required")
            .MaximumLength(255)
            .WithMessage("OriginalName must not exceed 255 characters");

        RuleFor(x => x.StoragePath)
            .NotEmpty()
            .WithMessage("StoragePath is required")
            .MaximumLength(1000)
            .WithMessage("StoragePath must not exceed 1000 characters");

        When(x => x.SizeBytes.HasValue, () =>
        {
            RuleFor(x => x.SizeBytes!.Value)
                .GreaterThan(0)
                .WithMessage("SizeBytes must be greater than 0");
        });

        When(x => !string.IsNullOrWhiteSpace(x.AltText), () =>
        {
            RuleFor(x => x.AltText!)
                .MaximumLength(500)
                .WithMessage("AltText must not exceed 500 characters");
        });
    }
}
