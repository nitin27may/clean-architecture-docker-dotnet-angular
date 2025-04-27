using Contact.Domain.Entities;
using System.Data;

namespace Contact.Domain.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role> GetRoleByName(string roleName, IDbTransaction? transaction = null);
    Task<bool> DeleteRole(Guid id, IDbTransaction? transaction = null);
    Task<Role> GetRoleById(Guid id, IDbTransaction? transaction = null);
}
