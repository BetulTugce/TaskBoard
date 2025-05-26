using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
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
        readonly HttpClient _httpClient;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService, ITokenHandler tokenHandler, IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _tokenHandler = tokenHandler;
            _mapper = mapper;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
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
                    user.LastName = surname ?? user.LastName; // Facebook ile gelen istekte surname bilgisi ayri verilmiyor (name icinde yer aliyor), bu nedenle null kontrolu yapiliyor..
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
                        EmailConfirmed = true,
                    };
                    // Facebook ile gelen talepte surname bilgisi ayri verilmiyor (name icinde yer aliyor), bu nedenle null kontrolu yapiliyor..
                    if (!string.IsNullOrEmpty(surname))
                    {
                        user.LastName = surname;
                    }
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
            var response = await CreateUserExternalAsync(user, payload.Email, payload.GivenName, payload.FamilyName, info, 3600);
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

        // Facebook ile giris yapar.. Facebook API kullanilarak kullanicinin bilgileri alinir ve kullanici veritabaninda kayitli degilse yeni bir kullanici olusturulur. Ve geriye bir token dondurulur..
        public async Task<Result<FacebookLoginResponseDto>> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            // Facebook uygulama erisim tokeni aliniyor..
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:App_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:App_Secret"]}&grant_type=client_credentials");

            FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

            // authToken, kullanicinin Facebooktan aldigi token.
            // Uygulama tokeni ile beraber facebook graph apiye dogrulatiliyor..
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

            FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                //Facebook kullanici idsi ve provider adiyla sistemde daha once login olmus kullanici var mi kontrol ediliyor..
                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                ApplicationUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                var response =  await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, string.Empty, info, accessTokenLifeTime);
                return Result<FacebookLoginResponseDto>.Success(new FacebookLoginResponseDto { Token = response }, "Successfully logged in!");
            }
            throw new Exception("Invalid external authentication.");
        }
    }
}
