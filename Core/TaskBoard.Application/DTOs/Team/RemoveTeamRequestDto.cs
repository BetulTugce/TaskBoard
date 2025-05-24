namespace TaskBoard.Application.DTOs.Team
{
    public class RemoveTeamRequestDto
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
    }
}
