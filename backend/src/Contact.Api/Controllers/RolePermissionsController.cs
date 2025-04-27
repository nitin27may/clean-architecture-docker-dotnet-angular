using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionsController(IRolePermissionService rolePermissionService) : ControllerBase
{
    // Controller currently has no active endpoints
    // Using primary constructor for dependency injection
}
