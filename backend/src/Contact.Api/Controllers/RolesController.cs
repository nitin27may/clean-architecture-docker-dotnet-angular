using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpPost]
    [AuthorizePermission("Roles.Create")]
    [ActivityLog("Creating new Role")]
    public async Task<IActionResult> AddRole(CreateRole createRole) => 
        Ok(await roleService.Add(createRole));

    [HttpPut("{id}")]
    [AuthorizePermission("Roles.Update")]
    [ActivityLog("Updating Role")]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateRole updateRole)
    {
        updateRole.Id = id;
        return Ok(await roleService.Update(updateRole));
    }

    [HttpGet]
    [AuthorizePermission("Roles.Read")]
    [ActivityLog("Fetching all Roles")]
    public async Task<IActionResult> GetRoles() => 
        Ok(await roleService.FindAll());

    [HttpDelete("{id}")]
    [AuthorizePermission("Roles.Delete")]
    [ActivityLog("Deleting Role")]
    public async Task<IActionResult> DeleteRole(Guid id) => 
        Ok(await roleService.Delete(id));
}
