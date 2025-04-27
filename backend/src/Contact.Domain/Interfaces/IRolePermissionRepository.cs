using Contact.Domain.Entities;
using Contact.Domain.Mappings;
using System.Data;


namespace Contact.Domain.Interfaces;

public interface IRolePermissionRepository : IGenericRepository<RolePermission>
{
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync();
    Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId);
    Task DeletePermissionsByRoleId(Guid roleId, IDbTransaction? transaction = null);
    Task<PermissionsByRoleMappings> GetRolePermissionMappingByRoleIdAsync(Guid roleId, IDbTransaction? transaction = null);
}
