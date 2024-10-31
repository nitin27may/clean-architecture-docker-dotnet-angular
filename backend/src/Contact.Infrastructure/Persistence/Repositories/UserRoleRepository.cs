using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(IDapperHelper dapperHelper, IOptions<AppSettings> appSettings) : base(dapperHelper, "UserRoles")
    {
    }
    public new async Task<UserRole> Add(UserRole userRole, IDbTransaction transaction = null)
    {

        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", userRole.UserId);
        dbPara.Add("RoleId", userRole.RoleId);
        dbPara.Add("CreatedOn", userRole.CreatedOn);
        dbPara.Add("CreatedBy", userRole.CreatedBy);

        return await _dapperHelper.Insert<UserRole>("INSERT INTO UserRoles (UserId,RoleId,CreatedOn,CreatedBy) VALUES(@UserId,@RoleId, @CreatedOn, @CreatedBy)",
                            dbPara, CommandType.Text, transaction);
    }
}
