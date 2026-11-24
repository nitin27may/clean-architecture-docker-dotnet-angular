using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

public sealed class AuthenticateHandler(IUserService userService) : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
{
    public Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        => userService.Authenticate(request);
}
