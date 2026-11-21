using Contact.Application.Abstractions.Messaging;
using Contact.Application.Interfaces;
using Contact.Application.Mapping;
using Contact.Domain.Exceptions;

namespace Contact.Application.UseCases.Users;

public sealed class RegisterUserHandler(IUserService userService, IMapper mapper)
    : IRequestHandler<RegisterUser, UserResponse>
{
    public async Task<UserResponse> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var existing = await userService.CheckUniqueUsers(request.Email, request.Username);
        if (existing.Any())
        {
            throw new BusinessException("User already exists");
        }

        var created = await userService.Add(request)
            ?? throw new BusinessException("User Details are not correct");

        // Map to the response DTO: returning the User entity sent the password hash to the client.
        return mapper.Map<UserResponse>(created);
    }
}
