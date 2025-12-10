using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class UserRepository(IDapperHelper dapperHelper, IOptions<AppSettings> appSettings) 
    : GenericRepository<User>(dapperHelper, "Users"), 
      IUserRepository
{
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<User> AddUser(User item, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("FirstName", item.FirstName, DbType.String);
        dbPara.Add("LastName", item.LastName, DbType.String);
        dbPara.Add("UserName", item.UserName, DbType.String);
        dbPara.Add("Email", item.Email, DbType.String);
        dbPara.Add("Mobile", item.Mobile, DbType.Int32);
        dbPara.Add("Password", item.Password, DbType.String);
        dbPara.Add("CreatedOn", item.CreatedOn, DbType.DateTimeOffset);
        dbPara.Add("CreatedBy", item.CreatedBy, DbType.Guid);

        return await dapperHelper.Insert<User>(@"
            INSERT INTO ""Users"" (""FirstName"", ""LastName"", ""UserName"", ""Email"", ""Mobile"", ""Password"", ""CreatedOn"", ""CreatedBy"")
            VALUES (@FirstName, @LastName, @UserName, @Email, @Mobile, @Password, @CreatedOn, @CreatedBy)
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<User> FindByID(int id, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", id);
        return await dapperHelper.Get<User>(@"SELECT * FROM ""Users"" WHERE ""Id"" = @Id", dbPara, CommandType.Text, transaction);
    }

    public async Task<IEnumerable<User>> CheckUniqueUsers(string email, string username, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Email", email);
        dbPara.Add("UserName", username);
        return await dapperHelper.GetAll<User>(@"
            SELECT * FROM ""Users"" 
            WHERE ""Email"" = @Email OR ""UserName"" = @UserName",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<IEnumerable<Role>> FindRolesById(Guid id, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", id);
        var sql = @"
            SELECT r.""Id"",
                r.""Name"",
                r.""Description""
            FROM ""UserRoles"" ur
            INNER JOIN ""Roles"" r ON ur.""RoleId"" = r.""Id""
            INNER JOIN ""Users"" u ON u.""Id"" = ur.""UserId""
            WHERE ur.""UserId"" = @UserId";
        return await dapperHelper.GetAll<Role>(sql, dbPara, CommandType.Text, transaction);
    }

    public async Task<User> FindByUserName(string userName, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserName", userName, DbType.String);
        return await dapperHelper.Get<User>(@"SELECT * FROM ""Users"" WHERE ""UserName"" = @UserName", dbPara, CommandType.Text, transaction);
    }

    public new async Task<User> Update(User item, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", item.Id);
        dbPara.Add("FirstName", item.FirstName, DbType.String);
        dbPara.Add("LastName", item.LastName, DbType.String);
        dbPara.Add("UserName", item.UserName, DbType.String);
        dbPara.Add("Email", item.Email, DbType.String);
        dbPara.Add("Mobile", item.Mobile);
        dbPara.Add("UpdatedBy", item.UpdatedBy, DbType.Guid);
        dbPara.Add("UpdatedOn", item.UpdatedOn, DbType.DateTimeOffset);
        return await dapperHelper.Update<User>(@"
            UPDATE ""Users"" 
            SET 
                ""FirstName"" = @FirstName,  
                ""LastName"" = @LastName, 
                ""UserName"" = @UserName, 
                ""Email"" = @Email,  
                ""Mobile"" = @Mobile,
                ""UpdatedBy"" = @UpdatedBy,
                ""UpdatedOn"" = @UpdatedOn
            WHERE ""Id"" = @Id
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<User> UpdatePassword(User item, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", item.Id);
        dbPara.Add("Password", item.Password);
        dbPara.Add("UpdatedBy", item.UpdatedBy, DbType.Guid);
        dbPara.Add("UpdatedOn", item.UpdatedOn, DbType.DateTimeOffset);
        return await dapperHelper.Update<User>(@"
            UPDATE ""Users"" 
            SET 
                ""Password"" = @Password,
                ""UpdatedBy"" = @UpdatedBy,
                ""UpdatedOn"" = @UpdatedOn
            WHERE ""Id"" = @Id
            RETURNING *",
            dbPara, CommandType.Text, transaction);
    }

    public async Task<IEnumerable<UserRole>> GetUserRoles(Guid userId, IDbTransaction? transaction = null)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", userId);

        var sql = @"
            SELECT *
            FROM ""UserRoles""
            WHERE ""UserId"" = @UserId";

        return await dapperHelper.GetAll<UserRole>(sql, dbPara, CommandType.Text, transaction);
    }
}
