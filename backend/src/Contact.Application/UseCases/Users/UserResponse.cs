using Contact.Application.UseCases.RolePermissions;

namespace Contact.Application.UseCases.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public List<RolePermissionResponse> RolePermissions { get; set; }

}
