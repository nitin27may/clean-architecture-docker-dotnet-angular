using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;

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
            return await _dapperHelper.GetAll<PageOperationMapping>(sql, null);
        }
    }
}
