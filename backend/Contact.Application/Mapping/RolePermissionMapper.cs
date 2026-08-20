using Contact.Application.UseCases.RolePermissions;
using Contact.Domain.Entities;
using Contact.Domain.Mappings;
using Riok.Mapperly.Abstractions;

namespace Contact.Application.Mapping;

[Mapper]
public partial class RolePermissionMapper
{
    // The bare entity carries none of the joined display fields (RoleName, PageName, ...) —
    // only the RolePermissionMapping projection below does. Matches the original
    // CreateMap<RolePermission, RolePermissionResponse>() with no ForMember configuration.
    [MapperIgnoreTarget(nameof(RolePermissionResponse.RoleName))]
    [MapperIgnoreTarget(nameof(RolePermissionResponse.PageName))]
    [MapperIgnoreTarget(nameof(RolePermissionResponse.PageOrder))]
    [MapperIgnoreTarget(nameof(RolePermissionResponse.PageUrl))]
    [MapperIgnoreTarget(nameof(RolePermissionResponse.OperationName))]
    public partial RolePermissionResponse ToRolePermissionResponse(RolePermission source);

    [MapperIgnoreSource(nameof(RolePermissionMapping.PageId))]
    [MapperIgnoreSource(nameof(RolePermissionMapping.OperationId))]
    public partial RolePermissionResponse ToRolePermissionResponse(RolePermissionMapping source);

    public partial RolePermission ToRolePermission(CreateRolePermission source, DateTimeOffset createdOn, Guid createdBy);

    // See UserMapper.ToUser(UpdateUser, ...) for why the audit placeholders are safe here.
    public partial RolePermission ToRolePermission(UpdateRolePermission source, DateTimeOffset createdOn, Guid createdBy);

    // Nested projection for the combined role -> pages -> operations tree used by
    // GetRolePermissionMappingByRoleIdAsync. Mapperly resolves the nested List<> members
    // (Pages, Operations) automatically using the other partial methods declared here.
    public partial RolePermissionMappingResponse ToRolePermissionMappingResponse(PermissionsByRoleMappings source);

    public partial PageOperationResponse ToPageOperationResponse(PageOperationMappings source);

    public partial OperationResponse ToOperationResponse(OperationMappings source);
}
