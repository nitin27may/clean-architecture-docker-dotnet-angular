using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRolesController(IUserService userService) : ControllerBase
{
    [HttpGet("{userId}")]
    [AuthorizePermission("UserRoles.Read")]
    [ActivityLog("Getting user roles")]
    public async Task<IActionResult> GetUserWithRoles(Guid userId)
    {
        var userWithRoles = await userService.GetUserWithRolesAsync(userId);
        return userWithRoles is null ? NotFound() : Ok(userWithRoles);
    }

    [HttpPut("{userId}/roles")]
    [AuthorizePermission("UserRoles.Update")]
    [ActivityLog("Updating user roles")]
    public async Task<IActionResult> UpdateUserRoles(Guid userId, UpdateUserRoles updateUserRoles) => 
        Ok(await userService.UpdateUserRoles(userId, updateUserRoles));
}
