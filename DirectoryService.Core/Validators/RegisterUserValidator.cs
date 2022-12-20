using DirectoryService.Core.Dto;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using FluentValidation;

namespace DirectoryService.Core.Validators;

// ReSharper disable once ClassNeverInstantiated.Global

[ScopedDependency]
public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        var config = ServiceConfigurationContainer.Config.Registration;
        var profanityFilter = new ProfanityFilter.ProfanityFilter();
        RuleFor(m => m.Username).NotEmpty().WithMessage("{PropertyName} not provided.")
            .MinimumLength(config.MinUsernameLength).WithMessage("{PropertyName} must be a minimum of " + config.MinUsernameLength + " characters")
            .MaximumLength(config.MaxUsernameLength).WithMessage("{PropertyName} must be a maximum of " + config.MaxUsernameLength + "characters")
            .Matches(@"^[A-Za-z0-9._-]+$").WithMessage("{PropertyName} can only contain letters, numbers, hyphens, dashes, periods.")
            .Custom((s, context) =>
            {
                if(profanityFilter.IsProfanity(s.ToLower()))
                {
                    context.AddFailure("{PropertyName} contains a blacklisted word.");
                }
            });
        
        RuleFor(m => m.Email).NotEmpty().WithMessage("{PropertyName} provided.")
            .EmailAddress().WithMessage("Email not valid.");

        RuleFor(m => m.Password).NotEmpty().WithMessage("Your password cannot be empty.");
    }
}