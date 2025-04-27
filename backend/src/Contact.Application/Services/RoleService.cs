using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class RoleService : GenericService<Role, RoleResponse, CreateRole, UpdateRole>, IRoleService
{
    public RoleService(IGenericRepository<Role> repository, IMapper mapper, IUnitOfWork unitOfWork)
          : base(repository, mapper, unitOfWork)
    {
    }
}