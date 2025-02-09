namespace TaskBoard.Application.DTOs.Team
{
    public class RemoveUserFromTeamDto
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
