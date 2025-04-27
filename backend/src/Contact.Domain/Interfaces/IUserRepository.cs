using Contact.Domain.Entities;
using System.Data;

namespace Contact.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
   Task<User> AddUser(User item, IDbTransaction? transaction = null);
    Task<User> FindByUserName(string userName, IDbTransaction? transaction = null);
    Task<IEnumerable<Role>> FindRolesById(Guid id, IDbTransaction? transaction = null);
    Task<IEnumerable<User>> CheckUniqueUsers(string email, string username, IDbTransaction? transaction = null);
    Task<User> UpdatePassword(User item, IDbTransaction? transaction = null);
    Task<IEnumerable<UserRole>> GetUserRoles(Guid userId, IDbTransaction? transaction = null);
}
