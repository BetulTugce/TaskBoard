using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.Abstractions.Token;
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
        public async Task<LoginUserResponseDto> LoginAsync(LoginUserRequestDto loginDto, int accessTokenLifeTime)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded) // Authentication basariliysa..
            {
                // Token olusturuluyor..
                Token token = _tokenHandler.GenerateAccessToken(accessTokenLifeTime, user);

                //TODO: Refresh Token bilgisi users tablosuna kaydedilecek..

                return new LoginUserResponseDto
                {
                    Token = token,
                    //User = _mapper.Map<LoginUserDto>(user)
                };
            }

            throw new Exception("Başarısız! Lütfen bilgilerinizi kontrol edin.");
        }
    }
}
