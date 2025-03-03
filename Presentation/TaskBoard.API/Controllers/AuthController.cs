﻿using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Abstractions.Services;
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
            var response = await _authService.LoginAsync(request, 1800);
            return Ok(response);
        }
    }
}
