using Microsoft.AspNetCore.Identity;
using TaskBoard.Application.Abstractions.Services;
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
        public async Task<CreateUserResponse> CreateAsync(CreateUserDto user)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }, user.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }
    }
}
