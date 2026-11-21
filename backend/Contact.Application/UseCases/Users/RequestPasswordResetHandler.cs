using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

public sealed class RequestPasswordResetHandler(IUserService userService) : IRequestHandler<RequestPasswordReset, bool>
{
    public Task<bool> Handle(RequestPasswordReset request, CancellationToken cancellationToken)
        => userService.ForgotPassword(request.Email);
}
