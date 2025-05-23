using TaskBoard.Application.DTOs;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<LoginUserResponseDto> LoginAsync(LoginUserRequestDto loginDto, int accessTokenLifeTime);
        Task<LoginUserResponseDto> RefreshTokenLoginAsync(RefreshTokenLoginRequestDto request);
    }
}
