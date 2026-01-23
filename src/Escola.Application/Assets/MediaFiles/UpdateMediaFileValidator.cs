using FluentValidation;

namespace Escola.Application.Assets.MediaFiles;

public class UpdateMediaFileValidator : AbstractValidator<UpdateMediaFileDto>
{
    public UpdateMediaFileValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.AltText) || !string.IsNullOrWhiteSpace(x.Metadata))
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.AltText), () =>
        {
            RuleFor(x => x.AltText!)
                .MaximumLength(500)
                .WithMessage("AltText must not exceed 500 characters");
        });
    }
}
