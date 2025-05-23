using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskBoard.Application.Abstractions.Token;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token GenerateAccessToken(int second, ApplicationUser user)
        {
            Application.DTOs.Token token = new();

            //Security Key'in simetrigi aliniyor..
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Authentication:User:SecurityKey"]));

            //Sifrelenmis kimligi olusturuyor..
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Kullanici bilgilerini iceren claim listesi olusturuluyor..
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            //Olusturulacak token ayarlari..
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken securityToken = new(
                //audience: _configuration["Token:Audience"],
                audience: _configuration["Authentication:User:Audience"],
                issuer: _configuration["Authentication:User:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
                //claims: new List<Claim> { new(ClaimTypes.Name, user.UserName) }
                );

            //Token olusturucu sınifindan bir ornek aliniyor..
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = GenerateRefreshToken();
            return token;
        }

        public string GenerateRefreshToken()
        {
            // 32 bytelik rastgele bir dizi olusturuyor..
            byte[] number = new byte[32];

            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);

            //Base64 formatina cevirerek geri dondurur..
            return Convert.ToBase64String(number);
        }
    }
}
