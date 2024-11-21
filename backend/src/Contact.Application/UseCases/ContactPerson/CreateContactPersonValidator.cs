using FluentValidation;

namespace Contact.Application.UseCases.ContactPerson;

public class CreateContactPersonValidator : AbstractValidator<CreateContactPerson>
{
    public CreateContactPersonValidator()
    {
        RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage("First Name is required")
              .MinimumLength(2).WithMessage("First Name must be at least 2 characters long")
              .MaximumLength(50).WithMessage("First Name must be less than or equal to 50 characters");

        // LastName validation: not empty, not null, length between 2 and 50 characters
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required")
            .MinimumLength(2).WithMessage("Last Name must be at least 2 characters long")
            .MaximumLength(50).WithMessage("Last Name must be less than or equal to 50 characters");

        // DateOfBirth validation: must be a past date
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required")
            .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past");

        // Mobile validation: 10 digits mobile number (you can customize this to suit different formats)
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile number is required")
            .InclusiveBetween(1000000000, 9999999999).WithMessage("Mobile number must be 10 digits");

        // Email validation: valid email format
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
