using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Contact.Api.Core.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var _rolePermissionService = scope.ServiceProvider.GetRequiredService<IRolePermissionService>();

            if (context.User.Identity.IsAuthenticated)
            {
                var userId = _userService.GetUserId(context.User); // Extract user ID from claims
                var roles = await _userService.GetUserRolesAsync(context.User); // Fetch roles for the user

                var rolePermissionMappings = await _rolePermissionService.GetRolePermissionMappingsAsync();
                var userPermissions = rolePermissionMappings
                    .Where(rpm => roles.Contains(rpm.RoleName))
                    .Select(rpm => $"{rpm.PageName}.{rpm.OperationName}Policy");

                if (userPermissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
