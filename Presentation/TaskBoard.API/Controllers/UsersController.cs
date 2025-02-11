using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto request)
        {
            CreateUserResponse response = await _userService.CreateAsync(request);
            if (response.Succeeded)
            {
                return StatusCode((int)HttpStatusCode.Created, response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
