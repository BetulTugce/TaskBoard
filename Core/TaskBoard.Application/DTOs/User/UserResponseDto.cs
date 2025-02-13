using TaskBoard.Application.DTOs.Task;
using TaskBoard.Application.DTOs.Team;

namespace TaskBoard.Application.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        // Kullanicinin gorevleri
        public List<TaskResponseDto> Tasks { get; set; } = new List<TaskResponseDto>();

        // Kullanicinin yonetici oldugu takimlar
        public List<TeamResponseDto> ManagedTeams { get; set; } = new List<TeamResponseDto>();

        // Kullanicinin dahil oldugu takimlar
        public List<TeamResponseDto> Teams { get; set; } = new List<TeamResponseDto>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
