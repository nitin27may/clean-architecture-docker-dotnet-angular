using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class PageMapper
{
    public partial Page ToPage(CreatePage source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    public partial Page ToPage(UpdatePage source, DateTimeOffset createdOn, Guid createdBy);

    public partial PageResponse ToPageResponse(Page source);
}
