using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private readonly IActivityLogService _activityLogService;
    private readonly IMessageService _messageService;
    private readonly INotificationService _notificationService;

    public UsersController(IUserService userService, IActivityLogService activityLogService, IMessageService messageService, INotificationService notificationService)
    {
        _userService = userService;
        _activityLogService = activityLogService;
        _messageService = messageService;
        _notificationService = notificationService;
    }

    [HttpPost("register")]

    public async Task<IActionResult> Register(RegisterUser createUser)
    {
        //var user = _mapper.Map<User>(createUser);
        var users = await _userService.CheckUniqueUsers(createUser.Email, createUser.Username);
        if (users.Any())
            return BadRequest(new { message = "User already exists" });
        var response = await _userService.Add(createUser);

        if (response == null)
            return BadRequest(new { message = "User Details are not correct" });
        return CreatedAtAction(nameof(GetCurrentUser), response);
    }
    [HttpPost("create")]
    [ActivityLog("Creating new User")]
    [AuthorizePermission("Admin.Create")]
    public async Task<IActionResult> Create(CreateUser createUser)
    {
        //var user = _mapper.Map<User>(createUser);
        var users = await _userService.CheckUniqueUsers(createUser.Email, createUser.Email);
        if (users.Any())
            return BadRequest(new { message = "User already exists" });
        var response = await _userService.Create(createUser);

        if (response == null)
            return BadRequest(new { message = "User Details are not correct" });
        return CreatedAtAction(nameof(GetCurrentUser), response);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token" });
        }

        var userId = Guid.Parse(userIdClaim);
        var result = await _userService.GetUserWithPermissionsAsync(userId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUser updateUser)
    {
        var user = await _userService.FindByID(id);
        if (user == null) return NotFound();
        var result = await _userService.Update(updateUser);
        return Ok(result);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model);

        if (response.Authenticate == false)
            return Unauthorized(new { message = "Username or password is incorrect" });

        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ForgetPassword(string email)
    {
        var result = await _userService.ForgotPassword(email);
        return Ok(result);
    }

    [HttpPost("confirm-reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
    {
        var result = await _userService.ResetPassword(resetPassword);
        return Ok(result);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
    {
        var result = await _userService.ChangePassword(changePassword);
        return Ok(result);
    }

    [HttpGet("activity-logs")]
    [Authorize]
    [AuthorizePermission("ActivityLog.Read")]
    public async Task<IActionResult> GetActivityLogs([FromQuery] string username = "", [FromQuery] string email = "")
    {
        // Ensure parameters are not null
        username = username ?? "";
        email = email ?? "";
        
        // Trim parameters to remove any whitespace
        username = username.Trim();
        email = email.Trim();
        
        try
        {
            var logs = await _activityLogService.GetActivityLogsAsync(username, email);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error retrieving activity logs: {ex.Message}" });
        }
    }

    [HttpPost("send-message")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SendMessage(SendMessage sendMessage)
    {
        await _messageService.SendMessage(sendMessage);
        return Ok();
    }

    [HttpGet("notifications")]
    [Authorize]
    public async Task<IActionResult> GetUserNotifications()
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token" });
        }

        var userId = Guid.Parse(userIdClaim);
        var notifications = await _notificationService.GetUserNotifications(userId);
        return Ok(notifications);
    }

    [HttpPut("notifications/{id}/read")]
    [Authorize]
    public async Task<IActionResult> MarkNotificationAsRead(Guid id)
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token" });
        }

        var userId = Guid.Parse(userIdClaim);
        await _notificationService.MarkAsRead(userId, id);
        return NoContent();
    }
}
