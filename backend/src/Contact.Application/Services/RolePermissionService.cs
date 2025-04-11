using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Application.UseCases.RolePermissions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contact.Application.Services;

public class RolePermissionService : GenericService<RolePermission, RolePermissionResponse, CreateRolePermission, UpdateRolePermission>, IRolePermissionService
{
    private readonly IGenericRepository<RolePermission> _repository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    
    public RolePermissionService(
        IGenericRepository<RolePermission> repository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork,
        IRolePermissionRepository rolePermissionRepository)
        : base(repository, mapper, unitOfWork)
    {
        _repository = repository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy)
    {
        foreach (var permissionId in permissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _repository.Add(rolePermission);
        }
    }

    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync()
    {
        return await _rolePermissionRepository.GetRolePermissionMappingsAsync();
    }

    public async Task<IEnumerable<RolePermissionMapping>> GetRolePermissionMappingsAsync(Guid userId)
    {
        return await _rolePermissionRepository.GetRolePermissionMappingsAsync(userId);
    }
}
