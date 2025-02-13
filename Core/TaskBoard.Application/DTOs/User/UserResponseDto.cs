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
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();

        // Kullanicinin yonetici oldugu takimlar
        public List<TeamDto> ManagedTeams { get; set; } = new List<TeamDto>();

        // Kullanicinin dahil oldugu takimlar
        public List<TeamDto> Teams { get; set; } = new List<TeamDto>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
