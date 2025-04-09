using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Roles")
    {
    }

    public async Task<Role> GetRoleByName(string roleName, IDbTransaction? transaction = null)
    {
        var rolePara = new DynamicParameters();
        rolePara.Add("Name", roleName);
        return await _dapperHelper.Get<Role>(@"SELECT * FROM ""Roles"" WHERE ""Name"" = @Name", rolePara, CommandType.Text, transaction);
    }
}
