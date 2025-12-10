using AutoMapper;
using Contact.Application.UseCases.Operations;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class OperationMappingProfile : BaseMappingProfile
{
     public OperationMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<Operation, OperationResponse>();
          
        CreateMap<CreateOperation, Operation>()
          .AfterMap((src, dest) => SetAuditFields(src, dest));
        CreateMap<UpdateOperation, Operation>()
          .AfterMap((src, dest) => SetAuditFields(src, dest, false));
    }
        public OperationMappingProfile()
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
