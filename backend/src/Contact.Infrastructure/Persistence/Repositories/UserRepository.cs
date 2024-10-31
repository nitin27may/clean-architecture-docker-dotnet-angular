using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;


public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppSettings _appSettings;
    public UserRepository(IDapperHelper dapperHelper, IOptions<AppSettings> appSettings) : base(dapperHelper, "Users")
    {
        _appSettings = appSettings.Value;
    }
    public async Task<User> AddUser(User item)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("FirstName", item.FirstName, DbType.String);
        dbPara.Add("LastName", item.LastName, DbType.String);
        dbPara.Add("Username", item.Username, DbType.String);
        dbPara.Add("Email", item.Email, DbType.String);
        dbPara.Add("Mobile", item.Mobile, DbType.Int32);
        dbPara.Add("Password", item.Password, DbType.String);
        dbPara.Add("CreatedOn", item.CreatedOn, DbType.DateTimeOffset);

        //dbPara.Add("CreatedBy", item.CreatedBy, DbType.Guid);

        return await _dapperHelper.Insert<User>("INSERT INTO Users(FirstName,LastName, Username, Email, Mobile, Password,CreatedOn) VALUES(@FirstName,@LastName,@Username,@Email,@Mobile,@Password, @CreatedOn)",
                            dbPara);
    }

    public async Task<User> FindByID(int id)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Id", id);
        return await _dapperHelper.Get<User>("SELECT * FROM where Users id = @Id", dbPara);
    }

    public async Task<IEnumerable<User>> CheckUniqueUsers(string email, string username)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("Email", email);
        dbPara.Add("UserName", username);
        return await _dapperHelper.GetAll<User>("SELECT * FROM Users   where Email = @email or UserName = @username;", dbPara);
    }

    public async Task<List<string>> FindRolesById(Guid id)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserId", id);
        var sql = @"
                SELECT r.Name 
                FROM UserRoles ur
                INNER JOIN Roles r ON ur.RoleId = r.Id
                Inner join Users u On u.Id = ur.UserId
                WHERE ur.UserId = @UserId";
        return (List<string>)await _dapperHelper.GetAll<string>(sql, dbPara);
    }


    public async Task<User> FindByUserName(string userName)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("UserName", userName, DbType.String);
        return await _dapperHelper.Get<User>("SELECT * FROM Users WHERE username = @UserName", dbPara);
    }

    public async Task<User> Update(User item)
    {
        var dbPara = new DynamicParameters();
        dbPara.Add("FirstName", item.FirstName, DbType.String);
        dbPara.Add("LastName", item.LastName, DbType.String);
        dbPara.Add("Username", item.Username, DbType.String);
        dbPara.Add("Email", item.Email, DbType.String);
        dbPara.Add("Mobile", item.Mobile, DbType.String);
        dbPara.Add("UpdatedBy", item.UpdatedBy, DbType.Guid);
        dbPara.Add("UpdatedOn", item.UpdatedOn, DbType.DateTimeOffset);
        return await _dapperHelper.Update<User>(@"
                        UPDATE Users 
                            SET 
                                firstname = @FirstName,  
                                lastname  = @LastName, 
                                username= @Username, 
                                email= @Email,  
                                mobile= @Mobile 
                             WHERE id = @Id",
                            dbPara);
    }
}
