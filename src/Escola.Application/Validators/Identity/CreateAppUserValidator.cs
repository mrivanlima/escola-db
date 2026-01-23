using Escola.Application.DTOs.Identity;
using FluentValidation;
using System.Text.Json;

namespace Escola.Application.Validators.Identity;

/// <summary>
/// Validator for CreateAppUserDto.
/// </summary>
public class CreateAppUserValidator : AbstractValidator<CreateAppUserDto>
{
    public CreateAppUserValidator()
    {
        RuleFor(x => x.TenantUuid)
            .NotEmpty()
            .WithMessage("Tenant UUID is required.");

        RuleFor(x => x.AuthUserId)
            .NotEmpty()
            .WithMessage("Auth User ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(200)
            .WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.UserRoleUuid)
            .NotEmpty()
            .WithMessage("User Role UUID is required.");

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
