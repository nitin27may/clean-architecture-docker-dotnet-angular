using System.Data;
using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;


public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<IEnumerable<PageOperationMapping>> GetPageOperationMappingsAsync(IDbTransaction? transaction = null);
}
