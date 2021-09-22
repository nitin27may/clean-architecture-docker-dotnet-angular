﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Repository;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var userDetails = await _userRepository.FindByEmail(user.Username);
            if (userDetails != null){
                return BadRequest(new { message = "user name/email is already exist" });
            }
            user.Email = user.Username;
            var response = await _userRepository.Add(user);

            if (response == null)
                return BadRequest(new { message = "user details are not correct" });

            return Ok(response);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userRepository.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
