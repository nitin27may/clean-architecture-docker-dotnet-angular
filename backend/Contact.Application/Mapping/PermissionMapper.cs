using Contact.Application.UseCases.Permissions;
using Contact.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class PermissionMapper
{
    // Permission (the raw entity) carries no PageName/OperationName — only the
    // PageOperationMapping projection below does. This mirrors the original
    // CreateMap<Permission, PermissionResponse>() with no ForMember configuration:
    // the two fields are left at their default (null) when mapped from the bare entity.
    [MapperIgnoreTarget(nameof(PermissionResponse.PageName))]
    [MapperIgnoreTarget(nameof(PermissionResponse.OperationName))]
    public partial PermissionResponse ToPermissionResponse(Permission source);

    public partial PermissionResponse ToPermissionResponse(PageOperationMapping source);

    public partial Permission ToPermission(CreatePermission source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    // UpdatePermission.UpdatedBy is mapped through by name but is overwritten afterwards
    // by ObjectMapper's audit stamping, exactly as the original AfterMap did.
    //
    // UpdatePermission carries no Description, and — unlike CreatedOn/CreatedBy/Password
    // elsewhere in this migration — GenericRepository's UPDATE statement does NOT exclude
    // Description, so a placeholder here would blank the column if this generic-service
    // Update() path is ever actually invoked (PermissionService's own UpdatePermission()
    // method bypasses the mapper entirely and does not touch Description, so it is safe).
    // AutoMapper had this same gap silently (it leaves unmatched members at their CLR
    // default), Mapperly's required-member check simply surfaces it. Tracked as a known
    // pre-existing issue rather than fixed here to keep this PR scoped to the mapper swap.
    public partial Permission ToPermission(UpdatePermission source, DateTimeOffset createdOn, Guid createdBy, string description);
}
