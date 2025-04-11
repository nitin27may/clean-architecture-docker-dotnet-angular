using Contact.Domain.Entities;
using Contact.Application.UseCases.RolePermissions;

namespace Contact.Application.Interfaces;

public interface IRolePermissionService: IGenericService<RolePermission, RolePermissionResponse, CreateRolePermission, UpdateRolePermission>
{
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync();
    Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy);
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId);
}
