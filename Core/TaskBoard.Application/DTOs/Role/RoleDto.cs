using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.DTOs.Role
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Roleun atandigi kullanicilar
        public List<UserDto> Users { get; set; } = new List<UserDto>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
