using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDapperHelper dapperHelper) : base(dapperHelper, "Permissions")
        {
        }

        public async Task<IEnumerable<PageOperationMapping>> GetPageOperationMappingsAsync()
        {
            var sql = @"
            SELECT
                p.""Name"" AS PageName,
                o.""Name"" AS OperationName
            FROM ""Permissions"" perm
            INNER JOIN ""Pages"" p ON perm.""PageId"" = p.""Id""
            INNER JOIN ""Operations"" o ON perm.""OperationId"" = o.""Id""
            ORDER BY p.""Name"", o.""Name"";";
            return await _dapperHelper.GetAll<PageOperationMapping>(sql, null, CommandType.Text);
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("PageId", permission.PageId);
            dbPara.Add("OperationId", permission.OperationId);
            dbPara.Add("Description", permission.Description);
            dbPara.Add("CreatedBy", permission.CreatedBy);
            dbPara.Add("CreatedOn", permission.CreatedOn);

            return await _dapperHelper.Insert<Permission>(@"
                INSERT INTO ""Permissions"" (""PageId"", ""OperationId"", ""Description"", ""CreatedBy"", ""CreatedOn"")
                VALUES (@PageId, @OperationId, @Description, @CreatedBy, @CreatedOn)
                RETURNING *",
                dbPara, CommandType.Text);
        }

        public async Task<Permission> UpdatePermission(Permission permission)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", permission.Id);
            dbPara.Add("PageId", permission.PageId);
            dbPara.Add("OperationId", permission.OperationId);
            dbPara.Add("Description", permission.Description);
            dbPara.Add("UpdatedBy", permission.UpdatedBy);
            dbPara.Add("UpdatedOn", permission.UpdatedOn);

            return await _dapperHelper.Update<Permission>(@"
                UPDATE ""Permissions"" 
                SET 
                    ""PageId"" = @PageId,  
                    ""OperationId"" = @OperationId, 
                    ""Description"" = @Description, 
                    ""UpdatedBy"" = @UpdatedBy,
                    ""UpdatedOn"" = @UpdatedOn
                WHERE ""Id"" = @Id
                RETURNING *",
                dbPara, CommandType.Text);
        }

        public async Task<bool> DeletePermission(Guid id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", id);

            var result = await _dapperHelper.Execute(@"
                DELETE FROM ""Permissions"" 
                WHERE ""Id"" = @Id",
                dbPara, CommandType.Text);

            return result > 0;
        }
    }
}
