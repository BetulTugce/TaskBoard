using Microsoft.AspNetCore.Mvc;
using TaskBoard.API.Extensions;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserRequestDto request)
        {
            // Yarim saatlik bir token olusturur..
            var result = await _authService.LoginAsync(request, 1800);

            if (!result.Succeeded)
                return this.MapErrorResult(result);
            //return Unauthorized(new { message = result.Message });

            return Ok(result.Data);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin(RefreshTokenLoginRequestDto request)
        {
            var result = await _authService.RefreshTokenLoginAsync(request);

            if (!result.Succeeded)
                return this.MapErrorResult(result);

            return Ok(result.Data);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult > GoogleLogin(GoogleLoginRequestDto request)
        {
            var result = await _authService.GoogleLoginAsync(request.IdToken);
            if (!result.Succeeded)
                return this.MapErrorResult(result);
            return Ok(result.Data);
        }
    }
}
