using FluentValidation;

namespace Contact.Application.UseCases.Users;

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("First is required.")
           .Length(1, 250).WithMessage("User name must be between 1 and 250 characters.");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("User Name is required.")
           .Length(1, 250).WithMessage("User name must be between 1 and 250 characters.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("User Name is required.")
            .Length(10, 100).WithMessage("User name must be between 10 and 100 characters.");

        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .Length(10, 200).WithMessage("User name must be between 10 and 200 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(15, 500).WithMessage("Password must be between 15 and 500 characters.");

    }
}
