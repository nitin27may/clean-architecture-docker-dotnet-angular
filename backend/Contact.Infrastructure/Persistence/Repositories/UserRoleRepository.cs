using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Infrastructure.Persistence.Helper;
using Dapper;
using System.Data;

namespace Contact.Infrastructure.Persistence.Repositories;

public class UserRoleRepository(IDapperHelper dapperHelper) 
    : GenericRepository<UserRole>(dapperHelper, "UserRoles"), 
      IUserRoleRepository
{
    // Additional specific methods can be implemented here if needed
}
