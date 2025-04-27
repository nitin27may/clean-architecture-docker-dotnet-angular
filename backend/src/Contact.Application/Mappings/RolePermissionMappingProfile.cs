using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;
using Contact.Domain.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class RolePermissionMappingProfile : BaseMappingProfile
{
    public RolePermissionMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<RolePermission, RolePermissionResponse>();
        CreateMap<RolePermissionMappingResponse, PermissionsByRoleMappings>();
        CreateMap<CreateRolePermission, RolePermission>()
            .AfterMap((src, dest) => SetAuditFields(src, dest));
        CreateMap<UpdateRolePermission, RolePermission>()
            .AfterMap((src, dest) => SetAuditFields(src, dest, false));
        // Configure the mapping between PermissionsByRoleMappings and RolePermissionMappingResponse
        CreateMap<PermissionsByRoleMappings, RolePermissionMappingResponse>();
        CreateMap<PageOperationMappings, PageOperationResponse>();
        CreateMap<OperationMappings, OperationResponse>();
    }

    public RolePermissionMappingProfile()
        : this(ResolveHttpContextAccessor())
    {
    }

    private static IHttpContextAccessor ResolveHttpContextAccessor()
    {
        var serviceProvider = new ServiceCollection()
            .AddHttpContextAccessor()
            .BuildServiceProvider();

        return serviceProvider.GetRequiredService<IHttpContextAccessor>();
    }
}

