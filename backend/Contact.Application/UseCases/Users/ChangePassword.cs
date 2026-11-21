using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>Changes the signed-in user's password (PUT /api/Users/change-password).</summary>
public class ChangePassword : IRequest<bool>
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
