using Contact.Application.UseCases.Permissions;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class PermissionMappingProfile : BaseMappingProfile
{
    public PermissionMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<Permission, PermissionResponse>();
        CreateMap<PageOperationMapping, PermissionResponse>();
        CreateMap<CreatePermission, Permission>()
         .AfterMap((src, dest) => SetAuditFields(src, dest));
        CreateMap<UpdatePermission, Permission>()
         .AfterMap((src, dest) => SetAuditFields(src, dest, false)); // Set audit fields on update
    }

    public PermissionMappingProfile()
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