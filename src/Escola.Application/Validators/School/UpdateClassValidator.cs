using Escola.Application.DTOs.School;
using FluentValidation;

namespace Escola.Application.Validators.School;

public class UpdateClassValidator : AbstractValidator<UpdateClassDto>
{
    public UpdateClassValidator()
    {
        RuleFor(x => x.ClassName).MinimumLength(2).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.ClassName));
        RuleFor(x => x.MaxStudents).GreaterThan(0).When(x => x.MaxStudents.HasValue);
    }
}
