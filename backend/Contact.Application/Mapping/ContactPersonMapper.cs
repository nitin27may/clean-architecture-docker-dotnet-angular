using Contact.Application.UseCases.ContactPerson;
using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class ContactPersonMapper
{
    public partial ContactPerson ToContactPerson(CreateContactPerson source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    public partial ContactPerson ToContactPerson(UpdateContactPerson source, DateTimeOffset createdOn, Guid createdBy);

    public partial ContactPersonResponse ToContactPersonResponse(ContactPerson source);
}
