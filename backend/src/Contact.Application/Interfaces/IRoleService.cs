using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IRoleService: IGenericService<Role, RoleResponse, CreateRole, UpdateRole>
{
  
}
