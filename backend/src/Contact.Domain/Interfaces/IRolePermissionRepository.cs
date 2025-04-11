using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;

public interface IRolePermissionRepository : IGenericRepository<RolePermission>
{
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync();
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId);
}
