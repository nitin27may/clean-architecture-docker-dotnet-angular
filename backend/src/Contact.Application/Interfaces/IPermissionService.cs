using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;


public interface IPermissionService
{
    Task<IEnumerable<PageOperationMapping>> GetAllPageOperationMappingsAsync();
}
