using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(IDapperHelper dapperHelper) : base(dapperHelper, "RolePermissions")
    {
    }

    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync()
    {
        var dbPara = new DynamicParameters();
        var sql = @"
            SELECT
                rp.""Id"",
                r.""Name"" AS RoleName,
                r.""Id"" AS RoleId,
                perm.""Id"" AS PermissionId,
                p.""Name"" AS PageName,
                p.""Url"" as PageUrl,
                o.""Name"" AS OperationName
            FROM ""RolePermissions"" rp
            INNER JOIN ""Roles"" r ON rp.""RoleId"" = r.""Id""
            INNER JOIN ""Permissions"" perm ON rp.""PermissionId"" = perm.""Id""
            INNER JOIN ""Pages"" p ON perm.""PageId"" = p.""Id""
            INNER JOIN ""Operations"" o ON perm.""OperationId"" = o.""Id""
            ORDER BY r.""Name"", p.""Name"", o.""Name"";";
        return await _dapperHelper.GetAll<RolePermissionMapping>(sql, dbPara);
    }

    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("@UserId", userId);
        var sql = @"
            SELECT
                r.""Id"" as RoleId,
                r.""Name"" AS RoleName,
                p.""Id"" as PageId,
                p.""Name"" AS PageName,
                p.""Url"" as PageUrl,
                o.""Id"" as OperationId,
                o.""Name"" AS OperationName
            FROM ""UserRoles"" ur
            INNER JOIN ""Roles"" r ON ur.""RoleId"" = r.""Id""
            INNER JOIN ""RolePermissions"" rp ON r.""Id"" = rp.""RoleId""
            INNER JOIN ""Permissions"" perm ON rp.""PermissionId"" = perm.""Id""
            INNER JOIN ""Pages"" p ON perm.""PageId"" = p.""Id""
            INNER JOIN ""Operations"" o ON perm.""OperationId"" = o.""Id""
            WHERE ur.""UserId"" = @UserId
            ORDER BY r.""Name"", p.""Name"", o.""Name"";";
        return await _dapperHelper.GetAll<RolePermissionMapping>(sql, dbPara);
    }

    public async Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy, IDbTransaction? transaction = null)
    {
        foreach (var permissionId in permissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await Add(rolePermission, transaction);
        }
    }



    public async Task DeletePermissionsByRoleId(Guid roleId, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("@RoleId", roleId);
        var sql = @"
            Delete FROM ""RolePermissions""
            WHERE ""RoleId"" = @RoleId;";
        await _dapperHelper.Execute(sql, dbPara, CommandType.Text, transaction);
    }
}
