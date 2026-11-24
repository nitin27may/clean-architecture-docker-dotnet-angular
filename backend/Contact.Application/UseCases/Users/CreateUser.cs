using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>A user created by an administrator (POST /api/Users/create). The user sets a password via the emailed link.</summary>
public class CreateUser : IRequest<UserResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

}
