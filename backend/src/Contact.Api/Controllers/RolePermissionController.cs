using Contact.Application.Interfaces;
using Contact.Application.UseCases.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;

    public RolePermissionController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignPermissionsToRole(Guid roleId, IEnumerable<Guid> permissionIds, Guid createdBy)
    {
        await _rolePermissionService.AssignPermissionsToRoleAsync(roleId, permissionIds, createdBy);
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
