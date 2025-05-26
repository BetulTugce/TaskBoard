using TaskBoard.Application.Common;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthentication
    {
        Task<Result<GoogleLoginResponseDto>> GoogleLoginAsync(string idToken);
        Task<Result<FacebookLoginResponseDto>> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
    }
}
