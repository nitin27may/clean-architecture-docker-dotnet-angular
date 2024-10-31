using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;

namespace Contact.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(IDapperHelper dapperHelper) : base(dapperHelper, "RolePermissions")
    {
    }


    // add Society Id to get the Role and Permissons
    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync()
    {
        var dbPara = new DynamicParameters();
        var sql = @"
            SELECT
                r.Id,
                r.Name AS RoleName,
                p.Name AS PageName,
                o.Name AS OperationName
            FROM RolePermissions rp
            INNER JOIN Roles r ON rp.RoleId = r.Id
            INNER JOIN Permissions perm ON rp.PermissionId = perm.Id
            INNER JOIN Pages p ON perm.PageId = p.Id
            INNER JOIN Operations o ON perm.OperationId = o.Id
            ORDER BY r.Name, p.Name, o.Name;";
        return await _dapperHelper.GetAll<RolePermissionMapping>(sql, dbPara);
    }

}
