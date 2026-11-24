using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

public sealed record GetAllUsers : IRequest<IEnumerable<UserResponse>>;
