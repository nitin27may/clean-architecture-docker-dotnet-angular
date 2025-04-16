using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionsController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;

    public RolePermissionsController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignPermissionsToRole(Guid roleId, IEnumerable<Guid> permissionIds)
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token" });
        }

        var userId = Guid.Parse(userIdClaim);
        //  await _rolePermissionService.AssignPermissionsToRoleAsync(roleId, permissionIds, userId);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRolePermissionMappings()
    {
        var response = await _rolePermissionService.GetRolePermissionMappingsAsync();
        return Ok(response);
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRolePermissionMappingsByUserId(Guid userId)
    {
        var response = await _rolePermissionService.GetRolePermissionMappingsAsync(userId);
        return Ok(response);
    }
}
