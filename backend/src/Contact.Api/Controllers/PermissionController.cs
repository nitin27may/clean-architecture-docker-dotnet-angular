using Contact.Application.Interfaces;
using Contact.Application.UseCases.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddPermission(CreatePermission createPermission)
    {
        var response = await _permissionService.AddPermission(createPermission);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePermission(Guid id, UpdatePermission updatePermission)
    {
        var response = await _permissionService.UpdatePermission(id, updatePermission);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPermissions()
    {
        var response = await _permissionService.GetPermissions();
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePermission(Guid id)
    {
        var response = await _permissionService.DeletePermission(id);
        return Ok(response);
    }
}
