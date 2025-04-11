using AutoMapper;
using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class RolePermissionMappingProfile : BaseMappingProfile
{
    public RolePermissionMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<RolePermission, RolePermissionResponse>();
        CreateMap<CreateRolePermission, RolePermission>()
            .AfterMap((src, dest) => SetAuditFields(src, dest));
        CreateMap<UpdateRolePermission, RolePermission>()
            .AfterMap((src, dest) => SetAuditFields(src, dest, false));
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