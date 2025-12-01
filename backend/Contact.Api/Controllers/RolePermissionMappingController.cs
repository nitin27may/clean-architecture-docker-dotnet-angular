using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.RolePermissions;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionMappingController(
    IRolePermissionService rolePermissionService,
    IRoleService roleService,
    IUserService userService,
    ILogger<RolePermissionMappingController> logger) : ControllerBase
{
    [HttpGet("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ActivityLog("Fetching all roles for permission mapping")]
     [AuthorizePermission("RolePermissionMapping.Read")]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await roleService.FindAll();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving roles");
            return StatusCode(500, "An error occurred while retrieving roles");
        }
    }

    [HttpGet("{roleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActivityLog("Fetching role permission mapping")]
    [AuthorizePermission("RolePermissionMapping.Update")]
    public async Task<IActionResult> GetRolePermissionMapping(Guid roleId)
    {
        try
        {
            var mapping = await rolePermissionService.GetRolePermissionMappingByRoleIdAsync(roleId);
            return Ok(mapping);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving role-permission mapping for role {RoleId}", roleId);
            return StatusCode(500, "An error occurred while retrieving role-permission mapping");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ActivityLog("Updated role-permission mapping")]
    public async Task<IActionResult> SaveRolePermissionMapping([FromBody] RolePermissionMappingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId is null)
                return Unauthorized(new { message = "User ID not found in token" });

            await rolePermissionService.SaveRolePermissionMappingAsync(request, Guid.Parse(userId));
            return Ok(new { message = "Role permissions saved successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving role-permission mapping for role {RoleId}", request.RoleId);
            return StatusCode(500, "An error occurred while saving role-permission mapping");
        }
    }
}