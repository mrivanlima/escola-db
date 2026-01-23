using Escola.Application.DTOs.Identity;
using FluentValidation;
using System.Text.Json;

namespace Escola.Application.Validators.Identity;

/// <summary>
/// Validator for UpdateAppUserDto.
/// </summary>
public class UpdateAppUserValidator : AbstractValidator<UpdateAppUserDto>
{
    public UpdateAppUserValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(200)
            .WithMessage("Full name cannot exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.UserConfig)
            .Must(BeValidJsonOrNull)
            .When(x => !string.IsNullOrWhiteSpace(x.UserConfig))
            .WithMessage("User Config must be valid JSON.");
    }

    private bool BeValidJsonOrNull(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
