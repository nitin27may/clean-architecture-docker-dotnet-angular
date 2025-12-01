using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, IActivityLogService activityLogService) : ControllerBase
{
    [HttpPost("register")]
   // [ActivityLog("User registration")]
    public async Task<IActionResult> Register(RegisterUser createUser)
    {
        var users = await userService.CheckUniqueUsers(createUser.Email, createUser.Username);
        if (users.Any())
            return BadRequest(new { message = "User already exists" });
            
        var response = await userService.Add(createUser);
        return response is null 
            ? BadRequest(new { message = "User Details are not correct" }) 
            : CreatedAtAction(nameof(GetCurrentUser), response);
    }

    [HttpPost("create")]
    [ActivityLog("Creating new User")]
    [AuthorizePermission("Users.Create")]
    public async Task<IActionResult> Create(CreateUser createUser)
    {
        var users = await userService.CheckUniqueUsers(createUser.Email, createUser.Email);
        if (users.Any())
            return BadRequest(new { message = "User already exists" });
            
        var response = await userService.Create(createUser);
        return response is null 
            ? BadRequest(new { message = "User Details are not correct" }) 
            : CreatedAtAction(nameof(GetCurrentUser), response);
    }

    [HttpGet]
    [Authorize]
    [ActivityLog("Fetching current user details")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (userIdClaim is null)
            return Unauthorized(new { message = "User ID not found in token" });

        var userId = Guid.Parse(userIdClaim);
        var result = await userService.GetUserWithPermissionsAsync(userId);
        return Ok(result);
    }

    [HttpGet("all")]
    [AuthorizePermission("Users.Read")]
    [Authorize]
    [ActivityLog("Fetching all users")]
    public async Task<IActionResult> GetAll() => 
        Ok(await userService.FindAll());

    [HttpPut("{id}")]
    [Authorize]
    [ActivityLog("Updating user details")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUser updateUser)
    {
        var user = await userService.FindByID(id);
        if (user is null) return NotFound();
        
        var result = await userService.Update(updateUser);
        return Ok(result);
    }

    [HttpPost("authenticate")]
  //  [ActivityLog("User authentication")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await userService.Authenticate(model);
        return response.Authenticate == false
            ? Unauthorized(new { message = "Username or password is incorrect" })
            : Ok(response);
    }

    [HttpPost("reset-password")]
   // [ActivityLog("Password reset request")]
    public async Task<IActionResult> ForgetPassword(string email) => 
        Ok(await userService.ForgotPassword(email));

    [HttpPost("confirm-reset-password")]
   // [ActivityLog("Confirming password reset")]
    public async Task<IActionResult> ResetPassword(ResetPassword resetPassword) => 
        Ok(await userService.ResetPassword(resetPassword));

    [HttpPut("change-password")]
    [Authorize]
    [ActivityLog("Changing user password")]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword) => 
        Ok(await userService.ChangePassword(changePassword));

    [HttpGet("activity-logs")]
    [Authorize]
    [AuthorizePermission("ActivityLog.Read")]
    [ActivityLog("Retrieving activity logs")]
    public async Task<IActionResult> GetActivityLogs([FromQuery] string username = "", [FromQuery] string email = "")
    {
        username = username?.Trim() ?? "";
        email = email?.Trim() ?? "";

        try
        {
            var logs = await activityLogService.GetActivityLogsAsync(username, email);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error retrieving activity logs: {ex.Message}" });
        }
    }
}
