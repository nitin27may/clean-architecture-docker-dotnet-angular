using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

/// <summary>The service identifies the signed-in user from the HTTP context, not from the request.</summary>
public sealed class ChangePasswordHandler(IUserService userService) : IRequestHandler<ChangePassword, bool>
{
    public Task<bool> Handle(ChangePassword request, CancellationToken cancellationToken)
        => userService.ChangePassword(request);
}
