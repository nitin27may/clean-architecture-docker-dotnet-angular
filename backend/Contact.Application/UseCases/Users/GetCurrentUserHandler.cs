using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

public sealed class GetCurrentUserHandler(IUserService userService) : IRequestHandler<GetCurrentUser, UserResponse?>
{
    public async Task<UserResponse?> Handle(GetCurrentUser request, CancellationToken cancellationToken)
        => await userService.GetUserWithPermissionsAsync(request.UserId);
}
