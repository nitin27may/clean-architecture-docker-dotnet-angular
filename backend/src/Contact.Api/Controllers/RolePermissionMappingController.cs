using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.RolePermissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contact.Api.Controllers;

[Route("api/role-permission-mapping")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class RolePermissionMappingController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;
    private readonly ILogger<RolePermissionMappingController> _logger;

    public RolePermissionMappingController(
        IRolePermissionService rolePermissionService,
        IRoleService roleService,
        IUserService userService,
        ILogger<RolePermissionMappingController> logger)
    {
        _rolePermissionService = rolePermissionService;
        _roleService = roleService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await _roleService.FindAll();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles");
            return StatusCode(500, "An error occurred while retrieving roles");
        }
    }

    [HttpGet("{roleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRolePermissionMapping(Guid roleId)
    {
        try
        {
            var mapping = await _rolePermissionService.GetRolePermissionMappingByRoleIdAsync(roleId);
            return Ok(mapping);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role-permission mapping for role {RoleId}", roleId);
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
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            await _rolePermissionService.SaveRolePermissionMappingAsync(request, Guid.Parse(userId));
            return Ok(new { message = "Role permissions saved successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving role-permission mapping for role {RoleId}", request.RoleId);
            return StatusCode(500, "An error occurred while saving role-permission mapping");
        }
    }
}