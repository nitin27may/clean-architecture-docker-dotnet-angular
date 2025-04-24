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
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IOperationRepository _operationRepository;
    private readonly IPageRepository _pageRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RolePermissionService(
        IGenericRepository<RolePermission> repository,
        IMapper mapper,
        IRolePermissionRepository rolePermissionRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IOperationRepository operationRepository,
        IPageRepository pageRepository,
        IUnitOfWork unitOfWork)
        : base(repository, mapper, unitOfWork)
    {
        _repository = repository;
        _rolePermissionRepository = rolePermissionRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _operationRepository = operationRepository;
        _pageRepository = pageRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds, Guid userId)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            // Delete existing permissions for the role
            await _rolePermissionRepository.DeletePermissionsByRoleId(roleId, transaction);

            // Assign new permissions
            foreach (var permissionId in permissionIds)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

                await _repository.Add(rolePermission, transaction);
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync()
    {
        return _mapper.Map<IEnumerable<RolePermissionResponse>>(await _rolePermissionRepository.GetRolePermissionMappingsAsync());
    }

    public async Task<IEnumerable<RolePermissionResponse>> GetRolePermissionMappingsAsync(Guid userId)
    {
        return _mapper.Map<IEnumerable<RolePermissionResponse>>(await _rolePermissionRepository.GetRolePermissionMappingsAsync(userId));
    }

    // New methods for combined role-permission mapping
    public async Task<RolePermissionMappingResponse> GetRolePermissionMappingByRoleIdAsync(Guid roleId)
    {
        // Get the role
        //  using var transaction = _unitOfWork.BeginTransaction();
        var result = await _rolePermissionRepository.GetRolePermissionMappingByRoleIdAsync(roleId);
        //  await _unitOfWork.CommitAsync();
        return _mapper.Map<RolePermissionMappingResponse>(result);
    }

    public async Task SaveRolePermissionMappingAsync(RolePermissionMappingRequest request, Guid userId)
    {
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            // Delete existing permissions for the role
            await _rolePermissionRepository.DeletePermissionsByRoleId(request.RoleId, transaction);

            // Get all permissions for mapping
            var allPermissions = await _permissionRepository.FindAll();

            // For each page in the request
            foreach (var pagePermission in request.Permissions)
            {
                // For each operation in the page
                foreach (var operationId in pagePermission.OperationIds)
                {
                    // Find the permission that matches this page-operation combination
                    var permission = allPermissions.FirstOrDefault(p =>
                        p.PageId == pagePermission.PageId && p.OperationId == operationId);

                    if (permission != null)
                    {
                        // Create a new role permission
                        var rolePermission = new RolePermission
                        {
                            RoleId = request.RoleId,
                            PermissionId = permission.Id,
                            CreatedBy = userId,
                            CreatedOn = DateTime.UtcNow
                        };

                        // Save to database
                        await _repository.Add(rolePermission, transaction);
                    }
                    else
                    {
                        // add permission
                        var newPermission = new Permission
                        {
                            PageId = pagePermission.PageId,
                            OperationId = operationId,
                            Description = $"{pagePermission.PageId} - {operationId}",
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = userId
                        };
                        var createdPermission = await _permissionRepository.Add(newPermission, transaction);
                        // Create a new role permission
                        var rolePermission = new RolePermission
                        {
                            RoleId = request.RoleId,
                            PermissionId = createdPermission.Id,
                            CreatedBy = userId,
                            CreatedOn = DateTime.UtcNow
                        };
                        // Save to database
                        await _repository.Add(rolePermission, transaction);
                    }
                }
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}