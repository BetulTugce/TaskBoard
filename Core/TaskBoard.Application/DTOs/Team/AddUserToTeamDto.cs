namespace TaskBoard.Application.DTOs.Team
{
    public class AddUserToTeamDto
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
