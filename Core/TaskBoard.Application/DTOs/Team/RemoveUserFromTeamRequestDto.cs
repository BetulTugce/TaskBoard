namespace TaskBoard.Application.DTOs.Team
{
    public class RemoveUserFromTeamRequestDto
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
