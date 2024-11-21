using Contact.Domain.Entities;
using System.Data;

namespace Contact.Domain.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role> GetRoleByName(string roleName, IDbTransaction? transaction = null);

}
