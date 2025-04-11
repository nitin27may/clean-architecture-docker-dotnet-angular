using AutoMapper;
using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class RoleMappingProfile : BaseMappingProfile
{
    public RoleMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<Role, RoleResponse>();
        CreateMap<CreateRole, Role>()
            .AfterMap((src, dest) => SetAuditFields(src, dest));
        CreateMap<UpdateRole, Role>()
            .AfterMap((src, dest) => SetAuditFields(src, dest, false)); // Set audit fields on update
    }
    
    public RoleMappingProfile()
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