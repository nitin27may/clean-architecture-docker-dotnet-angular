using FluentValidation;

namespace Contact.Application.UseCases.Users;

/// <summary>
/// Profile fields. Mobile is length-checked only, not format-checked like registration, so users
/// whose stored number predates that rule can still save their profile.
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUser>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
            .Matches(UserNameRules.Pattern).WithMessage(UserNameRules.Message);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile number is required")
            .MaximumLength(20).WithMessage("Mobile number cannot exceed 20 characters");
    }
}
