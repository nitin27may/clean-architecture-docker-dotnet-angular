using FluentValidation;
using System.Text.RegularExpressions;

namespace Contact.Application.UseCases.Users;

public partial class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
            
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
            
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");
            
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile number is required")
            .MaximumLength(20).WithMessage("Mobile number cannot exceed 20 characters")
            .Matches(PhoneRegex()).WithMessage("Mobile number is not in a valid format");
            
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage("Username can only contain letters, numbers, dots, underscores, and dashes");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Must(password => password.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter")
            .Must(password => password.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter")
            .Must(password => password.Any(char.IsDigit)).WithMessage("Password must contain at least one number")
            .Must(password => password.Any(c => !char.IsLetterOrDigit(c))).WithMessage("Password must contain at least one special character");
    }

    [GeneratedRegex(@"^\+?[0-9]{10,15}$")]
    private static partial Regex PhoneRegex();
}
