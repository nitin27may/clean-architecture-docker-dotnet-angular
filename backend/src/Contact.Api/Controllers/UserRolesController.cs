using Contact.Application.Interfaces;
using Contact.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IUserService _userService;

    public UserRolesController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("{userId}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRoles(Guid userId, UpdateUserRoles updateUserRoles)
    {
        var response = await _userService.UpdateUserRoles(userId, updateUserRoles);
        return Ok(response);
    }
}
