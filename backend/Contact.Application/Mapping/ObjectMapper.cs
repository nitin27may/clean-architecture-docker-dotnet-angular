using System.Collections;
using Contact.Application.UseCases.ContactPerson;
using Contact.Application.UseCases.Operations;
using Contact.Application.UseCases.Pages;
using Contact.Application.UseCases.Permissions;
using Contact.Application.UseCases.Roles;
using Contact.Application.UseCases.Users;
using Contact.Domain.Entities;
using Contact.Domain.Mappings;
using Microsoft.AspNetCore.Http;
using CreateRolePermission = Contact.Application.UseCases.RolePermissions.CreateRolePermission;
using UpdateRolePermission = Contact.Application.UseCases.RolePermissions.UpdateRolePermission;
using RolePermissionResponse = Contact.Application.UseCases.RolePermissions.RolePermissionResponse;
using RolePermissionMappingResponse = Contact.Application.UseCases.RolePermissions.RolePermissionMappingResponse;

namespace Contact.Application.Mapping;

/// <summary>
/// Replaces AutoMapper's IMapper.Map&lt;TDestination&gt;(object) for this application.
/// Every object-to-object conversion is generated at compile time by the Mapperly
/// [Mapper] partial classes in this folder (UserMapper, RoleMapper, ...); this type only
/// does two things AutoMapper used to do implicitly:
///   1. dispatch by (source type, destination type) pair — including element-wise
///      dispatch for IEnumerable&lt;T&gt;/List&lt;T&gt; destinations, so every existing
///      `mapper.Map&lt;IEnumerable&lt;X&gt;&gt;(...)` / `Map&lt;List&lt;X&gt;&gt;(...)` call site
///      keeps working unchanged;
///   2. stamp audit fields (CreatedOn/CreatedBy on create, UpdatedOn/UpdatedBy on update)
///      the way the old BaseMappingProfile.SetAuditFields AfterMap hook did.
/// See docs/adr/0001-permissive-license-dependency-policy.md for why AutoMapper was
/// replaced instead of pinned or paid for.
/// </summary>
public sealed class ObjectMapper : IMapper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly UserMapper _userMapper = new();
    private readonly RoleMapper _roleMapper = new();
    private readonly PageMapper _pageMapper = new();
    private readonly OperationMapper _operationMapper = new();
    private readonly PermissionMapper _permissionMapper = new();
    private readonly RolePermissionMapper _rolePermissionMapper = new();
    private readonly ContactPersonMapper _contactPersonMapper = new();

    private readonly Dictionary<(Type Source, Type Destination), Func<object, object?>> _map;

    public ObjectMapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _map = new Dictionary<(Type, Type), Func<object, object?>>
        {
            // Users
            [(typeof(CreateUser), typeof(User))] = s =>
                _userMapper.ToUser((CreateUser)s, UtcNow, CurrentUserId, password: string.Empty),
            [(typeof(RegisterUser), typeof(User))] = s =>
                _userMapper.ToUser((RegisterUser)s, UtcNow, CurrentUserId),
            [(typeof(UpdateUser), typeof(User))] = s =>
                StampUpdated(_userMapper.ToUser((UpdateUser)s, default, Guid.Empty, password: string.Empty)),
            [(typeof(User), typeof(UserResponse))] = s =>
                _userMapper.ToUserResponse((User)s),
            [(typeof(User), typeof(UserWithRolesResponse))] = s =>
                _userMapper.ToUserWithRolesResponse((User)s),
            [(typeof(RolePermissionMapping), typeof(RolePermissionResponse))] = s =>
                _rolePermissionMapper.ToRolePermissionResponse((RolePermissionMapping)s),
            [(typeof(ChangePassword), typeof(UpdatePassword))] = s =>
                StampUpdated(_userMapper.ToUpdatePassword((ChangePassword)s, default, Guid.Empty)),

            // Roles
            [(typeof(Role), typeof(RoleResponse))] = s =>
                _roleMapper.ToRoleResponse((Role)s),
            [(typeof(CreateRole), typeof(Role))] = s =>
                _roleMapper.ToRole((CreateRole)s, UtcNow, CurrentUserId),
            [(typeof(UpdateRole), typeof(Role))] = s =>
                StampUpdated(_roleMapper.ToRole((UpdateRole)s, default, Guid.Empty)),

            // Pages
            [(typeof(Page), typeof(PageResponse))] = s =>
                _pageMapper.ToPageResponse((Page)s),
            [(typeof(CreatePage), typeof(Page))] = s =>
                _pageMapper.ToPage((CreatePage)s, UtcNow, CurrentUserId),
            [(typeof(UpdatePage), typeof(Page))] = s =>
                StampUpdated(_pageMapper.ToPage((UpdatePage)s, default, Guid.Empty)),

            // Operations
            [(typeof(Operation), typeof(OperationResponse))] = s =>
                _operationMapper.ToOperationResponse((Operation)s),
            [(typeof(CreateOperation), typeof(Operation))] = s =>
                _operationMapper.ToOperation((CreateOperation)s, UtcNow, CurrentUserId),
            [(typeof(UpdateOperation), typeof(Operation))] = s =>
                StampUpdated(_operationMapper.ToOperation((UpdateOperation)s, default, Guid.Empty)),

            // Permissions
            [(typeof(Permission), typeof(PermissionResponse))] = s =>
                _permissionMapper.ToPermissionResponse((Permission)s),
            [(typeof(PageOperationMapping), typeof(PermissionResponse))] = s =>
                _permissionMapper.ToPermissionResponse((PageOperationMapping)s),
            [(typeof(CreatePermission), typeof(Permission))] = s =>
                _permissionMapper.ToPermission((CreatePermission)s, UtcNow, CurrentUserId),
            [(typeof(UpdatePermission), typeof(Permission))] = s =>
                StampUpdated(_permissionMapper.ToPermission((UpdatePermission)s, default, Guid.Empty, description: string.Empty)),

            // Role permissions
            [(typeof(RolePermission), typeof(RolePermissionResponse))] = s =>
                _rolePermissionMapper.ToRolePermissionResponse((RolePermission)s),
            [(typeof(CreateRolePermission), typeof(RolePermission))] = s =>
                _rolePermissionMapper.ToRolePermission((CreateRolePermission)s, UtcNow, CurrentUserId),
            [(typeof(UpdateRolePermission), typeof(RolePermission))] = s =>
                StampUpdated(_rolePermissionMapper.ToRolePermission((UpdateRolePermission)s, default, Guid.Empty)),
            [(typeof(PermissionsByRoleMappings), typeof(RolePermissionMappingResponse))] = s =>
                _rolePermissionMapper.ToRolePermissionMappingResponse((PermissionsByRoleMappings)s),

            // Contact persons
            [(typeof(ContactPerson), typeof(ContactPersonResponse))] = s =>
                _contactPersonMapper.ToContactPersonResponse((ContactPerson)s),
            [(typeof(CreateContactPerson), typeof(ContactPerson))] = s =>
                _contactPersonMapper.ToContactPerson((CreateContactPerson)s, UtcNow, CurrentUserId),
            [(typeof(UpdateContactPerson), typeof(ContactPerson))] = s =>
                StampUpdated(_contactPersonMapper.ToContactPerson((UpdateContactPerson)s, default, Guid.Empty)),
        };
    }

    public TDestination Map<TDestination>(object? source)
    {
        if (source is null)
        {
            return default!;
        }

        var sourceType = source.GetType();
        var destinationType = typeof(TDestination);

        if (_map.TryGetValue((sourceType, destinationType), out var direct))
        {
            return (TDestination)direct(source)!;
        }

        // Collection destinations (IEnumerable<T>/List<T>/...) are not registered directly —
        // every element is dispatched through the same lookup above, one at a time. This is
        // what lets `Map<IEnumerable<UserResponse>>(users)` and `Map<List<RoleResponse>>(roles)`
        // keep working without a registry entry per collection shape.
        if (source is IEnumerable sourceEnumerable and not string
            && TryGetEnumerableElementType(destinationType, out var elementType))
        {
            return (TDestination)BuildList(sourceEnumerable, elementType);
        }

        throw new InvalidOperationException(
            $"No mapping registered from '{sourceType.Name}' to '{destinationType.Name}'. " +
            $"Add it to {nameof(ObjectMapper)}.");
    }

    private object BuildList(IEnumerable source, Type elementType)
    {
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType)!;

        foreach (var item in source)
        {
            if (item is null)
            {
                list.Add(null);
                continue;
            }

            // Same element type on both sides (e.g. List<RolePermissionResponse> ->
            // List<RolePermissionResponse>) is a plain copy, not a conversion.
            if (elementType.IsInstanceOfType(item))
            {
                list.Add(item);
                continue;
            }

            if (_map.TryGetValue((item.GetType(), elementType), out var mapItem))
            {
                list.Add(mapItem(item));
                continue;
            }

            throw new InvalidOperationException(
                $"No mapping registered from '{item.GetType().Name}' to '{elementType.Name}' " +
                $"(while mapping a collection). Add it to {nameof(ObjectMapper)}.");
        }

        return list;
    }

    private static bool TryGetEnumerableElementType(Type type, out Type elementType)
    {
        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            if (definition == typeof(List<>) ||
                definition == typeof(IEnumerable<>) ||
                definition == typeof(ICollection<>) ||
                definition == typeof(IReadOnlyList<>) ||
                definition == typeof(IReadOnlyCollection<>))
            {
                elementType = type.GetGenericArguments()[0];
                return true;
            }
        }

        elementType = typeof(object);
        return false;
    }

    private T StampUpdated<T>(T entity) where T : BaseEntity
    {
        entity.UpdatedOn = UtcNow;
        entity.UpdatedBy = CurrentUserId;
        return entity;
    }

    private DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    private Guid CurrentUserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst("Id") is { } claim
            ? Guid.Parse(claim.Value)
            : Guid.Empty;
}
