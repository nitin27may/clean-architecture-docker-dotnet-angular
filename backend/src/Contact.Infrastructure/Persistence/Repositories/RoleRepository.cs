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

    public async Task<Role> GetRoleById(Guid id, IDbTransaction? transaction = null)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await _dapperHelper.Get<Role>(@"SELECT * FROM ""Roles"" WHERE ""Id"" = @Id", parameters, CommandType.Text, transaction);
    }

    public async Task<Role> GetRoleByName(string roleName, IDbTransaction? transaction = null)
    {
        var rolePara = new DynamicParameters();
        rolePara.Add("Name", roleName);
        return await _dapperHelper.Get<Role>(@"SELECT * FROM ""Roles"" WHERE ""Name"" = @Name", rolePara, CommandType.Text, transaction);
    }

    public async Task<Role> AddRole(Role role, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Name", role.Name);
        dbPara.Add("Description", role.Description);
        dbPara.Add("CreatedBy", role.CreatedBy);
        dbPara.Add("CreatedOn", role.CreatedOn);

        return await _dapperHelper.Insert<Role>(@"
            INSERT INTO ""Roles"" (""Name"", ""Description"", ""CreatedBy"", ""CreatedOn"")
            VALUES (@Name, @Description, @CreatedBy, @CreatedOn)
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<Role> UpdateRole(Role role, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", role.Id);
        dbPara.Add("Name", role.Name);
        dbPara.Add("Description", role.Description);
        dbPara.Add("UpdatedBy", role.UpdatedBy);
        dbPara.Add("UpdatedOn", role.UpdatedOn);

        return await _dapperHelper.Update<Role>(@"
            UPDATE ""Roles"" 
            SET 
                ""Name"" = @Name,  
                ""Description"" = @Description, 
                ""UpdatedBy"" = @UpdatedBy,
                ""UpdatedOn"" = @UpdatedOn
            WHERE ""Id"" = @Id
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<bool> DeleteRole(Guid id, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", id);

        var result = await _dapperHelper.Execute(@"
            DELETE FROM ""Roles"" 
            WHERE ""Id"" = @Id",
            dbPara, CommandType.Text, transaction);

        return result > 0;
    }
}
