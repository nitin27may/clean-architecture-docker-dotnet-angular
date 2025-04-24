using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Permissions;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class PermissionService(
    IGenericRepository<Permission> repository,
    IPermissionRepository permissionRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : GenericService<Permission, PermissionResponse, CreatePermission, UpdatePermission>(repository, mapper, unitOfWork),
      IPermissionService
{
    //public async Task<Permission> AddPermission(CreatePermission createPermission)
    //{
    //    using var transaction = unitOfWork.BeginTransaction();
    //    try
    //    {
    //        var permission = new Permission
    //        {
    //            PageId = createPermission.PageId,
    //            Description = createPermission.Description,
    //            OperationId = createPermission.OperationId,
    //            CreatedOn = DateTime.UtcNow
    //        };

    //        var result = await repository.Add(permission, transaction);
    //        await unitOfWork.CommitAsync();
    //        return result;
    //    }
    //    catch
    //    {
    //        await unitOfWork.RollbackAsync();
    //        throw;
    //    }
    //}

    public async Task<Permission> UpdatePermission(Guid id, UpdatePermission updatePermission)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var permission = await repository.FindByID(id, transaction);
            if (permission == null)
            {
                throw new Exception("Permission not found");
            }

            permission.PageId = updatePermission.PageId;
            permission.OperationId = updatePermission.OperationId;
            permission.UpdatedOn = DateTime.UtcNow;

            var result = await repository.Update(permission, transaction);
            await unitOfWork.CommitAsync();
            return result;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeletePermission(Guid id)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var result = await repository.Delete(id, transaction);
            await unitOfWork.CommitAsync();
            return result;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Permission>> GetPermissions()
    {
        return await repository.FindAll();
    }

    public async Task<IEnumerable<PermissionResponse>> GetAllPageOperationMappingsAsync()
    {
        return mapper.Map<IEnumerable<PermissionResponse>>(await permissionRepository.GetPageOperationMappingsAsync());
    }
}