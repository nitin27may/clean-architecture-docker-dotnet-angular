using Contact.Domain.Entities;
using Contact.Application.UseCases.Permissions;

namespace Contact.Application.Interfaces;

public interface IPermissionService : IGenericService<Permission, PermissionResponse, CreatePermission, UpdatePermission>
{
   Task<IEnumerable<PageOperationMapping>> GetAllPageOperationMappingsAsync();
}
