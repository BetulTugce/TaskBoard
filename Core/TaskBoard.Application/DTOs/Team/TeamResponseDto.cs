using TaskBoard.Application.DTOs.Task;
using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.DTOs.Team
{
    // Bir takimi ya da takimlari listelemek icin kullanilacak DTO
    public class TeamResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ManagerId { get; set; }
        //public string ManagerName { get; set; }

        // Takim uyeleri
        public List<UserResponseDto> Members { get; set; } = new();
        public List<TaskResponseDto> Tasks { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
