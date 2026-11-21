using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Application.Mapping;
using Contact.Domain.Exceptions;

namespace Contact.Application.UseCases.Users;

public sealed class CreateUserHandler(IUserService userService, IMapper mapper)
    : IRequestHandler<CreateUser, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        // Checks the username as well as the email. The controller used to pass the email twice,
        // so a duplicate username was only caught by the database.
        var existing = await userService.CheckUniqueUsers(request.Email, request.UserName);
        if (existing.Any())
        {
            throw new BusinessException("User already exists");
        }

        var created = await userService.Create(request)
            ?? throw new BusinessException("User Details are not correct");

        // Map to the response DTO: returning the User entity sent the password hash to the client.
        return mapper.Map<UserResponse>(created);
    }
}
