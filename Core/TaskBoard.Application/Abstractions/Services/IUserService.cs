using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserDto user);
        Task<List<UserResponseDto>> GetAllUsersAsync(int page, int size);
        int TotalUsersCount { get; }
    }
}
