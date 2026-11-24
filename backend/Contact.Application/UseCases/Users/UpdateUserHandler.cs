using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

/// <summary>Who may update whom is decided by the controller (self, or the Users.Update permission).</summary>
public sealed class UpdateUserHandler(IUserService userService) : IRequestHandler<UpdateUser, UserResponse?>
{
    public async Task<UserResponse?> Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        var existing = await userService.FindByID(request.Id);
        if (existing is null)
        {
            return null;
        }

        return await userService.Update(request);
    }
}
