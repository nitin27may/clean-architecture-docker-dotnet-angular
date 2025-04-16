using Contact.Domain.Entities;
using System.Data;

namespace Contact.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> AddUser(User item);
    Task<User> FindByUserName(string userName);
    Task<List<Role>> FindRolesById(Guid id);
    Task<IEnumerable<User>> CheckUniqueUsers(string email, string username);
    Task<User> UpdatePassword(User item, IDbTransaction? transaction = null);
    Task<IEnumerable<UserRole>> GetUserRoles(Guid userId, IDbTransaction? transaction = null);
}
