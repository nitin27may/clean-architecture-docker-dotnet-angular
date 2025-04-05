using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IRolePermissionService
{
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync();
    Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy);
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId);
}
