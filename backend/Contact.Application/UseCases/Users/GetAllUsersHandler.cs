using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;

namespace Contact.Application.UseCases.Users;

public sealed class GetAllUsersHandler(IUserService userService) : IRequestHandler<GetAllUsers, IEnumerable<UserResponse>>
{
    public Task<IEnumerable<UserResponse>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        => userService.FindAll();
}
