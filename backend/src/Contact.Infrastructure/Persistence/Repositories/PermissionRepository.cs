using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class PermissionRepository(IDapperHelper dapperHelper) 
    : GenericRepository<Permission>(dapperHelper, "Permissions"), 
      IPermissionRepository
{
    public async Task<IEnumerable<PageOperationMapping>> GetPageOperationMappingsAsync(IDbTransaction? transaction = null)
    {
        var sql = @"
        SELECT
            perm.""Id"" AS Id,
            perm.""Description"" AS Description,
            p.""Name"" AS PageName,
            p.""Id"" AS PageId,
            o.""Name"" AS OperationName,
            o.""Id"" AS OperationId
        FROM ""Permissions"" perm
        INNER JOIN ""Pages"" p ON perm.""PageId"" = p.""Id""
        INNER JOIN ""Operations"" o ON perm.""OperationId"" = o.""Id""
        ORDER BY p.""Name"", o.""Name"";";
        
        return await dapperHelper.GetAll<PageOperationMapping>(sql, null, System.Data.CommandType.Text, transaction);
    }
}
