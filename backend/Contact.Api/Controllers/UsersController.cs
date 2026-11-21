using Contact.Api.Core.Attributes;
using Contact.Application.Abstractions.Messaging;
using Contact.Application.UseCases.ActivityLogs;
using Contact.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(ISender sender, IAuthorizationService authorizationService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUser registerUser, CancellationToken cancellationToken)
    {
        var response = await sender.Send(registerUser, cancellationToken);
        return CreatedAtAction(nameof(GetCurrentUser), response);
    }

    [HttpPost("create")]
    [ActivityLog("Creating new User")]
    [AuthorizePermission("Users.Create")]
    public async Task<IActionResult> Create(CreateUser createUser, CancellationToken cancellationToken)
    {
        var response = await sender.Send(createUser, cancellationToken);
        return CreatedAtAction(nameof(GetCurrentUser), response);
    }

    [HttpGet]
    [Authorize]
    [ActivityLog("Fetching current user details")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized(new { message = "User ID not found in token" });

        return Ok(await sender.Send(new GetCurrentUser(userId), cancellationToken));
    }

    [HttpGet("all")]
    [AuthorizePermission("Users.Read")]
    [Authorize]
    [ActivityLog("Fetching all users")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await sender.Send(new GetAllUsers(), cancellationToken));

    [HttpPut("{id}")]
    [Authorize]
    [ActivityLog("Updating user details")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUser updateUser, CancellationToken cancellationToken)
    {
        // Users may edit their own profile; editing anyone else needs the Users.Update permission.
        // Previously any signed-in user could change any user's details, including the email address
        // that password resets are sent to.
        var isSelf = TryGetCurrentUserId(out var currentUserId) && currentUserId == id;
        if (!isSelf && !(await authorizationService.AuthorizeAsync(User, "Users.UpdatePolicy")).Succeeded)
            return Forbid();

        // The route decides which user is updated, not an Id in the body.
        updateUser.Id = id;

        var result = await sender.Send(updateUser, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model, CancellationToken cancellationToken)
    {
        var response = await sender.Send(model, cancellationToken);
        return response.Authenticate == false
            ? Unauthorized(new { message = "Username or password is incorrect" })
            : Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ForgetPassword(string email, CancellationToken cancellationToken) =>
        Ok(await sender.Send(new RequestPasswordReset(email), cancellationToken));

    [HttpPost("confirm-reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPassword resetPassword, CancellationToken cancellationToken) =>
        Ok(await sender.Send(resetPassword, cancellationToken));

    [HttpPut("change-password")]
    [Authorize]
    [ActivityLog("Changing user password")]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword, CancellationToken cancellationToken) =>
        Ok(await sender.Send(changePassword, cancellationToken));

    [HttpGet("activity-logs")]
    [Authorize]
    [AuthorizePermission("ActivityLog.Read")]
    [ActivityLog("Retrieving activity logs")]
    public async Task<IActionResult> GetActivityLogs(
        CancellationToken cancellationToken, [FromQuery] string username = "", [FromQuery] string email = "") =>
        Ok(await sender.Send(new GetActivityLogs(username, email), cancellationToken));

    private bool TryGetCurrentUserId(out Guid userId) =>
        Guid.TryParse(User.FindFirst("Id")?.Value, out userId);
}
