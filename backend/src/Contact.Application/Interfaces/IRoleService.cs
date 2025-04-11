using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IRoleService
{
    Task<Role> AddRole(CreateRole createRole);
    Task<Role> UpdateRole(Guid id, UpdateRole updateRole);
    Task<IEnumerable<Role>> GetRoles();
    Task<bool> DeleteRole(Guid id);
}
