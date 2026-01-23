using FluentValidation;

namespace Escola.Application.Game.StudentProgress;

public class CreateStudentProgressValidator : AbstractValidator<CreateStudentProgressDto>
{
    public CreateStudentProgressValidator()
    {
        RuleFor(x => x.StudentUuid)
            .NotEmpty()
            .WithMessage("StudentUuid is required");

        RuleFor(x => x.ActivityUuid)
            .NotEmpty()
            .WithMessage("ActivityUuid is required");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters");

        When(x => x.Score.HasValue, () =>
        {
            RuleFor(x => x.Score!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Score must be 0 or greater");
        });

        RuleFor(x => x.Attempts)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Attempts must be 0 or greater");

        When(x => x.TimeSpentSeconds.HasValue, () =>
        {
            RuleFor(x => x.TimeSpentSeconds!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("TimeSpentSeconds must be 0 or greater");
        });
    }
}

public class UpdateStudentProgressValidator : AbstractValidator<UpdateStudentProgressDto>
{
    public UpdateStudentProgressValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Status) ||
                      x.Score.HasValue ||
                      x.Attempts.HasValue ||
                      x.TimeSpentSeconds.HasValue ||
                      !string.IsNullOrWhiteSpace(x.ProgressData) ||
                      x.CompletedAt.HasValue)
            .WithMessage("At least one field must be provided for update");

        When(x => !string.IsNullOrWhiteSpace(x.Status), () =>
        {
            RuleFor(x => x.Status!)
                .MaximumLength(50)
                .WithMessage("Status must not exceed 50 characters");
        });

        When(x => x.Score.HasValue, () =>
        {
            RuleFor(x => x.Score!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Score must be 0 or greater");
        });

        When(x => x.Attempts.HasValue, () =>
        {
            RuleFor(x => x.Attempts!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Attempts must be 0 or greater");
        });

        When(x => x.TimeSpentSeconds.HasValue, () =>
        {
            RuleFor(x => x.TimeSpentSeconds!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("TimeSpentSeconds must be 0 or greater");
        });
    }
}
