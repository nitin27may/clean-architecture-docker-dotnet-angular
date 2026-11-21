using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>Emails a password reset link (POST /api/Users/reset-password).</summary>
public sealed record RequestPasswordReset(string Email) : IRequest<bool>;
