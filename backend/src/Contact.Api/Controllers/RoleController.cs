using Contact.Application.Interfaces;
using Contact.Application.UseCases.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRole(CreateRole createRole)
    {
        var response = await _roleService.Add(createRole);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateRole updateRole)
    {
        updateRole.Id = id;
        var response = await _roleService.Update(updateRole);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRoles()
    {
        var response = await _roleService.FindAll();
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {;
        var response = await _roleService.Delete(id);
        return Ok(response);
    }
}
