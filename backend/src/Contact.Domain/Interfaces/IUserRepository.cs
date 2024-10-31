using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> AddUser(User item);
    Task<User> FindByUserName(string userName);
    Task<List<string>> FindRolesById(Guid id);
    Task<IEnumerable<User>> CheckUniqueUsers(string email, string username);
}
