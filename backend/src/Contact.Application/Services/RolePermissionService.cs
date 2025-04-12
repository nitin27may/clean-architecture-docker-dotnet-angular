using AutoMapper;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class RolePermissionService : GenericService<RolePermission, RolePermissionResponse, CreateRolePermission, UpdateRolePermission>, IRolePermissionService
{
    private readonly IGenericRepository<RolePermission> _repository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public RolePermissionService(
        IGenericRepository<RolePermission> repository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IRolePermissionRepository rolePermissionRepository)
        : base(repository, mapper, unitOfWork)
    {
        _repository = repository;
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy)
    {
        using var transaction = _unitOfWork.BeginTransaction();

        await _rolePermissionRepository.DeletePermissionsByRoleId(roleId, transaction);
        foreach (var permissionId in permissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _repository.Add(rolePermission, transaction);
        }
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync()
    {
        return _mapper.Map<IEnumerable<RolePermissionResponse>>(await _rolePermissionRepository.GetRolePermissionMappingsAsync());
    }

    public async Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync(Guid userId)
    {
        return _mapper.Map<IEnumerable<RolePermissionResponse>>(await _rolePermissionRepository.GetRolePermissionMappingsAsync(userId));
    }
}
