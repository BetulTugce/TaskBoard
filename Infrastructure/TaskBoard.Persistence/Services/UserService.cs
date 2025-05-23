using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Task;
using TaskBoard.Application.DTOs.User;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Yeni kullanici olusturur..
        public async Task<CreateUserResponseDto> CreateAsync(CreateUserRequestDto userDto)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            }, userDto.Password);

            CreateUserResponseDto response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }

        // Sistemdeki tum kullanicilari listeler..
        public async Task<List<UserResponseDto>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users.OrderBy(i => i.UserName).Include(u => u.Tasks)
                  .Skip((page - 1) * size)
                  .Take(size)
                  .ToListAsync();
            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Tasks = user.Tasks.Select(task => new TaskResponseDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt,
                }).ToList(),
            }).ToList();
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);

            await _userManager.UpdateAsync(user);
        }

        public int TotalUsersCount => _userManager.Users.Count();
    }
}
