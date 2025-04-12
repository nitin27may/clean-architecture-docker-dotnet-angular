using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IRolePermissionService : IGenericService<RolePermission, RolePermissionResponse, CreateRolePermission, UpdateRolePermission>
{
    Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync();
    Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy);
    Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync(Guid userId);
}
