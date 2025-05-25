using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, ITokenHandler tokenHandler, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _tokenHandler = tokenHandler;
            _mapper = mapper;
            _configuration = configuration;
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

        async Task<Token> CreateUserExternalAsync(ApplicationUser user, string email, string name, string surname, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if(user is not null)
                {
                    // Kullanici email ile bulunursa, kullanici bilgileri guncelleniyor..
                    user.EmailConfirmed = true;
                    user.FirstName = name;
                    user.LastName = surname;
                    var identityResult = await _userManager.UpdateAsync(user);
                    result = identityResult.Succeeded;
                }
                else
                {// Kullanici email ile bulunamadiysa, yeni bir kullanici olusturuluyor..
                    user = new()
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        UserName = email,
                        FirstName = name,
                        LastName = surname,
                        EmailConfirmed = true,
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

                Token token = _tokenHandler.GenerateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }
            throw new Exception("Invalid external authentication.");
        }

        public async Task<Result<GoogleLoginResponseDto>> GoogleLoginAsync(string idToken)
        {
            // Token dogrulama ayarlari yapiliyor.. Google.Apis.Auth kullanilarak gecerli bir token olup olmadigi kontrol ediliyor..
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] } // Google Client ID
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            // payload, Google'dan gelen token bilgilerini icerir. ASP.NET Core Identity sinifi olan UserLoginInfo kullanilarak bu kullanicinin harici (external) kimlik saglayicidan geldigi tanimlanir.
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            //  Google hesabiyla daha önce giris yapmis kullaniciyi bulmaya calisir.. 
            ApplicationUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            // Null donerse yeni bir kullanici olusturulacak demektir..Varsa giris yapacak.. 
            var response = await CreateUserExternalAsync(user, payload.Email, payload.GivenName, payload.FamilyName, info, 15);
            return Result<GoogleLoginResponseDto>.Success(new GoogleLoginResponseDto { Token = response }, "Successfully logged in!");
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
