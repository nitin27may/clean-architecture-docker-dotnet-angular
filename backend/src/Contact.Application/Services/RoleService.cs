using Contact.Application.Interfaces;
using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Role> AddRole(CreateRole createRole)
    {
        var role = new Role
        {
            Name = createRole.Name,
            Description = createRole.Description,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createRole.CreatedBy
        };

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var createdRole = await _roleRepository.Add(role, transaction);
            await _unitOfWork.CommitAsync();
            return createdRole;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<Role> UpdateRole(Guid id, UpdateRole updateRole)
    {
        var role = await _roleRepository.FindByID(id);
        if (role == null) throw new Exception("Role not found");

        role.Name = updateRole.Name;
        role.Description = updateRole.Description;
        role.UpdatedOn = DateTime.UtcNow;
        role.UpdatedBy = updateRole.UpdatedBy;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var updatedRole = await _roleRepository.Update(role, transaction);
            await _unitOfWork.CommitAsync();
            return updatedRole;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Role>> GetRoles()
    {
        return await _roleRepository.FindAll();
    }

    public async Task<bool> DeleteRole(Guid id)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var result = await _roleRepository.Delete(id, transaction);
            await _unitOfWork.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
