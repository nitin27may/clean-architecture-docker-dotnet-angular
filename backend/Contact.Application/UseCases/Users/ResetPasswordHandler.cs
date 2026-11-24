using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

public sealed class ResetPasswordHandler(IUserService userService) : IRequestHandler<ResetPassword, bool>
{
    public Task<bool> Handle(ResetPassword request, CancellationToken cancellationToken)
        => userService.ResetPassword(request);
}
