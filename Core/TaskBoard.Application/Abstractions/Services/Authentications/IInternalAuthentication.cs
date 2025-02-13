using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthentication
    {
        Task<LoginUserResponseDto> LoginAsync(LoginUserRequestDto loginDto, int accessTokenLifeTime);
    }
}
