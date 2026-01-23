using FluentValidation;

namespace Escola.Application.Game.StudentBadges;

public class AwardStudentBadgeValidator : AbstractValidator<AwardStudentBadgeDto>
{
    public AwardStudentBadgeValidator()
    {
        RuleFor(x => x.StudentUuid)
            .NotEmpty()
            .WithMessage("StudentUuid is required");

        RuleFor(x => x.BadgeUuid)
            .NotEmpty()
            .WithMessage("BadgeUuid is required");
    }
}
