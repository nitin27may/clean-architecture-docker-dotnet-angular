using Contact.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    // All endpoints are currently commented out
    // Using primary constructor for dependency injection
}
