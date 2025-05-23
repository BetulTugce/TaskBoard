using TaskBoard.Application.DTOs.User;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserRequestDto userDto);
        Task<List<UserResponseDto>> GetAllUsersAsync(int page, int size);
        int TotalUsersCount { get; }

        Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
    }
}
