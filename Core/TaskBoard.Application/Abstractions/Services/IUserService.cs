using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUserDto user);
    }
}
