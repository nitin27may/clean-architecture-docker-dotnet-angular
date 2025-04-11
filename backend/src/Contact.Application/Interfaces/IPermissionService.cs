using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PageOperationMapping>> GetAllPageOperationMappingsAsync();
    Task<Permission> AddPermission(CreatePermission createPermission);
    Task<Permission> UpdatePermission(Guid id, UpdatePermission updatePermission);
    Task<bool> DeletePermission(Guid id);
    Task<IEnumerable<Permission>> GetPermissions();
}
