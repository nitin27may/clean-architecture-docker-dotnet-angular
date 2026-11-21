using Contact.Application.Abstractions.Messaging;

namespace Contact.Application.UseCases.Users;

/// <summary>
/// Profile update (PUT /api/Users/{id}). <see cref="Id"/> is set from the route by the controller.
/// Responds with <c>null</c> when no user has that id.
/// </summary>
public class UpdateUser : IRequest<UserResponse?>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
}
