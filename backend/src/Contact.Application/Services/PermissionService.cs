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
}
