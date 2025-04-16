using Contact.Application.UseCases.RolePermissions;
using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;


public class UserMappingProfile : BaseMappingProfile
{
    public UserMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        // Define your mappings here
        CreateMap<CreateUser, User>()
                  .AfterMap((src, dest) => SetAuditFields(src, dest)); // Set audit fields on creation
        CreateMap<RegisterUser, User>()
                   .AfterMap((src, dest) => SetAuditFields(src, dest)); // Set audit fields on creation
        CreateMap<UpdateUser, User>()
            .AfterMap((src, dest) => SetAuditFields(src, dest, false)); // Set audit fields on update
        CreateMap<User, UserResponse>();
        CreateMap<User, UserWithRolesResponse>();
        CreateMap<RolePermissionMapping, RolePermissionResponse>();
        CreateMap<ChangePassword, UpdatePassword>()
          .AfterMap((src, dest) => SetAuditFields(src, dest, false)); // Set audit fields on update
    }

    public UserMappingProfile()
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
