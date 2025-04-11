using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Application.UseCases.Permissions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contact.Application.Services;

public class PermissionService : GenericService<Permission, PermissionResponse, CreatePermission, UpdatePermission>, IPermissionService
{
    private readonly IGenericRepository<Permission> _repository;
    private readonly IGenericRepository<PageOperationMapping> _pageOperationMappingRepository;
    
    public PermissionService(
        IGenericRepository<Permission> repository,
        IGenericRepository<PageOperationMapping> pageOperationMappingRepository,
        IMapper mapper, 
        IUnitOfWork unitOfWork)
        : base(repository, mapper, unitOfWork)
    {
        _repository = repository;
        _pageOperationMappingRepository = pageOperationMappingRepository;
    }

    public async Task<Permission> AddPermission(CreatePermission createPermission)
    {
        var permission = new Permission
        {
            PageId = createPermission.PageId,
            Description = createPermission.Description,
            OperationId = createPermission.OperationId,
            CreatedOn = DateTime.UtcNow
        };

        return await _repository.Add(permission);
    }

    public async Task<Permission> UpdatePermission(Guid id, UpdatePermission updatePermission)
    {
        var permission = await _repository.FindByID(id);
        if (permission == null)
        {
            throw new Exception("Permission not found");
        }

        permission.PageId = updatePermission.PageId;
        permission.OperationId = updatePermission.OperationId;
        permission.UpdatedOn = DateTime.UtcNow;

        return await _repository.Update(permission);
    }

    public async Task<bool> DeletePermission(Guid id)
    {
        return await _repository.Delete(id);
    }

    public async Task<IEnumerable<Permission>> GetPermissions()
    {
        return await _repository.FindAll();
    }

    public async Task<IEnumerable<PageOperationMapping>> GetAllPageOperationMappingsAsync()
    {
        return await _pageOperationMappingRepository.FindAll();
    }
}
