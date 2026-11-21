using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>Completes a password reset with the emailed code (POST /api/Users/confirm-reset-password).</summary>
public class ResetPassword : IRequest<bool>
{
    public required string Email { get; init; }
    public required string ResetCode { get; init; }
    public required string NewPassword { get; init; }
}
