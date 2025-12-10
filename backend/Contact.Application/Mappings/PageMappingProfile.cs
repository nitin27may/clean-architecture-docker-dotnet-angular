using AutoMapper;
using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class PageMappingProfile : BaseMappingProfile
{
    public PageMappingProfile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<Page, PageResponse>();
        CreateMap<CreatePage, Page>()
         .AfterMap((src, dest) => SetAuditFields(src, dest)); 
        CreateMap<UpdatePage, Page>()
         .AfterMap((src, dest) => SetAuditFields(src, dest, false)); // Set audit fields on update
    }
    
    public PageMappingProfile()
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