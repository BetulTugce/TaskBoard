using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserRequestDto userDto);
        Task<List<UserResponseDto>> GetAllUsersAsync(int page, int size);
        int TotalUsersCount { get; }
    }
}
