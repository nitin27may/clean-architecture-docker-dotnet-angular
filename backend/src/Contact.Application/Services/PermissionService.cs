using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<PageOperationMapping>> GetAllPageOperationMappingsAsync()
    {
        return await _permissionRepository.GetPageOperationMappingsAsync();
    }

    public async Task<Permission> AddPermission(CreatePermission createPermission)
    {
        var permission = new Permission
        {
            PageId = createPermission.PageId,
            OperationId = createPermission.OperationId,
            CreatedBy = createPermission.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        return await _permissionRepository.Add(permission);
    }

    public async Task<Permission> UpdatePermission(Guid id, UpdatePermission updatePermission)
    {
        var permission = await _permissionRepository.FindByID(id);
        if (permission == null)
        {
            throw new NotFoundException("Permission not found");
        }

        permission.PageId = updatePermission.PageId;
        permission.OperationId = updatePermission.OperationId;
        permission.UpdatedBy = updatePermission.UpdatedBy;
        permission.UpdatedOn = DateTime.UtcNow;

        return await _permissionRepository.Update(permission);
    }

    public async Task<bool> DeletePermission(Guid id)
    {
        return await _permissionRepository.Delete(id);
    }

    public async Task<IEnumerable<Permission>> GetPermissions()
    {
        return await _permissionRepository.FindAll();
    }
}
