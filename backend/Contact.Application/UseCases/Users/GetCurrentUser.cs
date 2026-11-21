using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>The signed-in user with their role permissions. Responds with <c>null</c> if the user no longer exists.</summary>
public sealed record GetCurrentUser(Guid UserId) : IRequest<UserResponse?>;
