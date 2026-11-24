using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>Self-registration (POST /api/Users/register). Validated by <see cref="RegisterUserValidator"/>.</summary>
public class RegisterUser : IRequest<UserResponse>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
