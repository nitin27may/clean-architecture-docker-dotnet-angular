using Contact.Application.UseCases.ContactPerson;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Application.Mappings;

public class ContactPersonPofile : BaseMappingProfile
{
    public ContactPersonPofile(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        CreateMap<ContactPerson, ContactPersonResponse>();
        
        CreateMap<CreateContactPerson, ContactPerson>()
            .AfterMap((src, dest) => SetAuditFields(src, dest));
            
        CreateMap<UpdateContactPerson, ContactPerson>()
            .AfterMap((src, dest) => SetAuditFields(src, dest, false));
    }

    public ContactPersonPofile()
        : this(ResolveHttpContextAccessor())
    {
    }

    private static IHttpContextAccessor ResolveHttpContextAccessor() => 
        new ServiceCollection()
            .AddHttpContextAccessor()
            .BuildServiceProvider()
            .GetRequiredService<IHttpContextAccessor>();
}
