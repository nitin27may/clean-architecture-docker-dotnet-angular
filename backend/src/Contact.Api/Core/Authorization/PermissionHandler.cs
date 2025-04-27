using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Contact.Api.Core.Authorization;

public class PermissionHandler(IServiceProvider serviceProvider) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var rolePermissionService = scope.ServiceProvider.GetRequiredService<IRolePermissionService>();

        if (context.User.Identity is { IsAuthenticated: true })
        {
            var userId = userService.GetUserId(context.User); // Extract user ID from claims
            var roles = await userService.GetUserRolesAsync(context.User); // Fetch roles for the user

            var rolePermissionMappings = await rolePermissionService.GetRolePermissionMappingsAsync();
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
