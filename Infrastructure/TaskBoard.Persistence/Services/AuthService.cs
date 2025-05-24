using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.Abstractions.Token;
using TaskBoard.Application.Common;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.DTOs.User;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly IUserService _userService;
        readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, ITokenHandler tokenHandler, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _tokenHandler = tokenHandler;
            _mapper = mapper;
        }

        // Username ya da email ve parola ile giris yapar..
        public async Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginDto, int accessTokenLifeTime)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);

            if (user == null)
                return Result<LoginUserResponseDto>.Failure("Please check your credentials!", ErrorCode.Unauthorized);

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded) // Authentication basariliysa..
            {
                // Token olusturuluyor..
                Token token = _tokenHandler.GenerateAccessToken(accessTokenLifeTime, user);

                // Kullaniciya ait refresh token veritabaninda guncelleniyor.
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);

                return Result<LoginUserResponseDto>.Success(new LoginUserResponseDto
                {
                    Token = token
                }, "Successfully logged in!");
            }

            return Result<LoginUserResponseDto>.Failure("Please check your password!", ErrorCode.Unauthorized);
        }

        public async Task<Result<LoginUserResponseDto>> RefreshTokenLoginAsync(RefreshTokenLoginRequestDto request)
        {
            // RefreshTokena sahip kullanici araniyor..
            ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenEndDate < DateTime.UtcNow)
                return Result<LoginUserResponseDto>.Failure("Refresh token is invalid or expired!", ErrorCode.Unauthorized); // Gecersiz veya suresi dolmus token

            // Token olusturuluyor.
            Token token = _tokenHandler.GenerateAccessToken(15, user);

            // User tablosundaki RefreshToken bilgisi olusturulan token bilgisinden alinarak guncelleniyor..
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
            LoginUserResponseDto response = new()
            {
                Token = token,
            };
            return Result<LoginUserResponseDto>.Success(response, "Successfully logged in!");
        }
    }
}
