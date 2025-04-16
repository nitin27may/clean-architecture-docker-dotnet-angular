using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IRolePermissionService : IGenericService<RolePermission, RolePermissionResponse, CreateRolePermission, UpdateRolePermission>
{
    Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync();
    Task AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds, Guid userId);
    Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync(Guid userId);
    
    // New methods for combined role-permission mapping
    Task<RolePermissionMappingResponse> GetRolePermissionMappingByRoleIdAsync(Guid roleId);
    Task SaveRolePermissionMappingAsync(RolePermissionMappingRequest request, Guid userId);
}
