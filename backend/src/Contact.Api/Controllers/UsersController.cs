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

    public UsersController(IUserService userService)
    {
        _userService = userService;
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
        var result = await _userService.FindAll();
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

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
    {
        var result = await _userService.ChangePassword(changePassword);
        return Ok(result);
    }
}