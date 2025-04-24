using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Domain.Mappings;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class RolePermissionRepository(IDapperHelper dapperHelper) 
    : GenericRepository<RolePermission>(dapperHelper, "RolePermissions"), 
      IRolePermissionRepository
{
    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync()
    {
        var sql = @"
            WITH page_operations AS (
                -- All possible page-operation combinations
                SELECT 
                    p.""Id"" as ""PageId"", 
                    p.""Name"" as ""PageName"",
                    p.""Url"" as ""PageUrl"",
                    o.""Id"" as ""OperationId"",
                    o.""Name"" as ""OperationName"",
                    perm.""Id"" as ""PermissionId""
                FROM ""Pages"" p
                CROSS JOIN ""Operations"" o
                LEFT JOIN ""Permissions"" perm ON p.""Id"" = perm.""PageId"" AND o.""Id"" = perm.""OperationId""
                -- Removed the WHERE clause to include ALL combinations
            ),
            role_permissions AS (
                -- Get permissions assigned to roles
                SELECT 
                    r.""Id"" as ""RoleId"",
                    r.""Name"" as ""RoleName"",
                    po.""PageId"",
                    po.""PageName"",
                    po.""PageUrl"",
                    po.""OperationId"",
                    po.""OperationName"",
                    po.""PermissionId"",
                    -- Only mark as selected if both permission exists AND role has it assigned
                    CASE 
                        WHEN po.""PermissionId"" IS NOT NULL AND rp.""Id"" IS NOT NULL THEN TRUE
                        ELSE FALSE
                    END as ""IsSelected"",
                    rp.""Id""
                FROM ""Roles"" r
                CROSS JOIN page_operations po
                -- Modified LEFT JOIN to handle NULL permission IDs
                LEFT JOIN ""RolePermissions"" rp ON po.""PermissionId"" = rp.""PermissionId"" AND rp.""RoleId"" = r.""Id"" AND po.""PermissionId"" IS NOT NULL
            )
            
            -- Final result
            SELECT 
                rp.""Id"",
                rp.""RoleId"",
                rp.""RoleName"",
                rp.""PageId"",
                rp.""PageName"",
                rp.""PageUrl"",
                rp.""OperationId"",
                rp.""OperationName"",
                rp.""PermissionId"",
                rp.""IsSelected""
            FROM role_permissions rp
            ORDER BY rp.""RoleName"", rp.""PageName"", rp.""OperationName""";

        return await dapperHelper.GetAll<RolePermissionMapping>(sql, new DynamicParameters());
    }

    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("@UserId", userId);
        var sql = @"
            SELECT
                rp.""Id"" as Id,
                r.""Id"" as RoleId,
                r.""Name"" AS RoleName,
                p.""Id"" as PageId,
                p.""Name"" AS PageName,
                p.""Url"" as PageUrl,
                p.""Order"" as PageOrder,
                o.""Id"" as OperationId,
                o.""Name"" AS OperationName,
                perm.""Id"" as PermissionId
            FROM ""UserRoles"" ur
            INNER JOIN ""Roles"" r ON ur.""RoleId"" = r.""Id""
            INNER JOIN ""RolePermissions"" rp ON r.""Id"" = rp.""RoleId""
            INNER JOIN ""Permissions"" perm ON rp.""PermissionId"" = perm.""Id""
            INNER JOIN ""Pages"" p ON perm.""PageId"" = p.""Id""
            INNER JOIN ""Operations"" o ON perm.""OperationId"" = o.""Id""
            WHERE ur.""UserId"" = @UserId
            ORDER BY r.""Name"", p.""Name"", o.""Name"";";
        return await dapperHelper.GetAll<RolePermissionMapping>(sql, dbPara);
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
        await dapperHelper.Execute(sql, dbPara, CommandType.Text, transaction);
    }

    public async Task<PermissionsByRoleMappings> GetRolePermissionMappingByRoleIdAsync(Guid roleId, IDbTransaction? transaction = null)
    {
        // SQL query for getting all data needed for the role permission mapping in one go
        var query = @"
            WITH role_info AS (
                SELECT r.""Id"" as ""RoleId"", r.""Name"" as ""RoleName""
                FROM ""Roles"" r
                WHERE r.""Id"" = @RoleId
            ),
            page_operations AS (
                -- All possible page-operation combinations
                SELECT 
                    p.""Id"" as ""PageId"", 
                    p.""Name"" as ""PageName"",
                    o.""Id"" as ""OperationId"",
                    o.""Name"" as ""OperationName"",
                    perm.""Id"" as ""PermissionId""
                FROM ""Pages"" p
                CROSS JOIN ""Operations"" o
                LEFT JOIN ""Permissions"" perm ON p.""Id"" = perm.""PageId"" AND o.""Id"" = perm.""OperationId""
                -- Removed the WHERE clause to include ALL combinations
            ),
            role_permissions AS (
                -- Get permissions assigned to the role
                SELECT 
                    po.""PageId"",
                    po.""PageName"",
                    po.""OperationId"",
                    po.""OperationName"",
                    
                    -- Only mark as selected if both permission exists AND role has it assigned
                    CASE 
                        WHEN po.""PermissionId"" IS NOT NULL AND rp.""Id"" IS NOT NULL THEN TRUE
                        ELSE FALSE
                    END as ""IsSelected""
                FROM page_operations po
                -- Modified LEFT JOIN to handle NULL permission IDs
                LEFT JOIN ""RolePermissions"" rp ON po.""PermissionId"" = rp.""PermissionId"" AND rp.""RoleId"" = @RoleId AND po.""PermissionId"" IS NOT NULL
            )
            
            -- Final result combining role info with permissions
            SELECT 
                r.""RoleId"",
                r.""RoleName"",
                rp.""PageId"",
                rp.""PageName"",
                rp.""OperationId"",
                rp.""OperationName"",
                rp.""IsSelected""
            FROM role_info r
            CROSS JOIN role_permissions rp
            ORDER BY rp.""PageName"", rp.""OperationName""";

        var parameters = new DynamicParameters();
        parameters.Add("RoleId", roleId, DbType.Guid);

        // Dictionary to hold the pages already added to the response
        var pagesDict = new Dictionary<Guid, PageOperationMappings>();

        // Create the response using Dapper's multi-mapping capabilities
        var response = await dapperHelper.GetAll<dynamic>(query, parameters, CommandType.Text, transaction);

        // If no data found, return null
        if (!response.Any())
        {
            // Query just the role to check if it exists
            var roleQuery = "SELECT \"Id\" as \"RoleId\", \"Name\" as \"RoleName\" FROM \"Roles\" WHERE \"Id\" = @RoleId";
            var role = await dapperHelper.Get<dynamic>(roleQuery, parameters, CommandType.Text, transaction);

            if (role == null)
                return null; // Role not found

            // Role exists but has no permissions, return empty mapping
            return new PermissionsByRoleMappings
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Pages = await GetAllPagesWithOperations(transaction)
            };
        }

        // Initialize the response object
        var mappingResponse = new PermissionsByRoleMappings
        {
            RoleId = response.First().RoleId,
            RoleName = response.First().RoleName,
            Pages = new List<PageOperationMappings>()
        };

        // Process each row from the query
        foreach (var row in response)
        {
            var pageId = (Guid)row.PageId;

            // Check if this page is already added to the dictionary
            if (!pagesDict.TryGetValue(pageId, out var pageResponse))
            {
                // Create new page response
                pageResponse = new PageOperationMappings
                {
                    PageId = pageId,
                    PageName = row.PageName,
                    Operations = new List<OperationMappings>()
                };

                // Add to dictionary and response list
                pagesDict[pageId] = pageResponse;
                mappingResponse.Pages.Add(pageResponse);
            }

            // Add operation to the page
            pageResponse.Operations.Add(new OperationMappings
            {
                OperationId = row.OperationId,
                OperationName = row.OperationName,
                IsSelected = row.IsSelected
            });
        }

        return mappingResponse;
    }

    private async Task<List<PageOperationMappings>> GetAllPagesWithOperations(IDbTransaction? transaction = null)
    {
        // Get all pages with operations, but set IsSelected to false
        var query = @"
            SELECT 
                p.""Id"" as ""PageId"", 
                p.""Name"" as ""PageName"",
                o.""Id"" as ""OperationId"",
                o.""Name"" as ""OperationName"",
                false as ""IsSelected""
            FROM ""Pages"" p
            CROSS JOIN ""Operations"" o
            LEFT JOIN ""Permissions"" perm ON p.""Id"" = perm.""PageId"" AND o.""Id"" = perm.""OperationId""
            -- Removed INNER JOIN to include all combinations
            ORDER BY p.""Name"", o.""Name""";

        var result = await dapperHelper.GetAll<dynamic>(query, null, CommandType.Text, transaction);

        var pagesDict = new Dictionary<Guid, PageOperationMappings>();
        var pagesList = new List<PageOperationMappings>();

        foreach (var row in result)
        {
            var pageId = (Guid)row.PageId;

            if (!pagesDict.TryGetValue(pageId, out var pageResponse))
            {
                pageResponse = new PageOperationMappings
                {
                    PageId = pageId,
                    PageName = row.PageName,
                    Operations = new List<OperationMappings>()
                };

                pagesDict[pageId] = pageResponse;
                pagesList.Add(pageResponse);
            }

            pageResponse.Operations.Add(new OperationMappings
            {
                OperationId = row.OperationId,
                OperationName = row.OperationName,
                IsSelected = false
            });
        }

        return pagesList;
    }
}
