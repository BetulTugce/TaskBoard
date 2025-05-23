using TaskBoard.Application.Common;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginDto, int accessTokenLifeTime);
        Task<Result<LoginUserResponseDto>> RefreshTokenLoginAsync(RefreshTokenLoginRequestDto request);
    }
}
