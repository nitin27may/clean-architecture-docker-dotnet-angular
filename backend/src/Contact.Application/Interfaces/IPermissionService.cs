using Contact.Application.UseCases.Permissions;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IPermissionService : IGenericService<Permission, PermissionResponse, CreatePermission, UpdatePermission>
{
    Task<IEnumerable<PermissionResponse>> GetAllPageOperationMappingsAsync();
}
