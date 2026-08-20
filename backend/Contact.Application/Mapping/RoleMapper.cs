using Contact.Application.UseCases.Roles;
using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class RoleMapper
{
    public partial Role ToRole(CreateRole source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    public partial Role ToRole(UpdateRole source, DateTimeOffset createdOn, Guid createdBy);

    public partial RoleResponse ToRoleResponse(Role source);
}
