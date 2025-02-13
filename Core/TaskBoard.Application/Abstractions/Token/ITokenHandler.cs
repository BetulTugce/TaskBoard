using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Application.Abstractions.Token
{
    public interface ITokenHandler
    {

        // Bir kullanici icin belli bir sure gecerli bir accesstoken olusturur..
        DTOs.Token GenerateAccessToken(int second, ApplicationUser user);

        // AccessToken gecerliligini kaybettiginde kullanilarak yeni bir token alinmasini saglar..
        string GenerateRefreshToken();
    }
}
