namespace TaskBoard.Application.DTOs.Team
{
    public class AddUserToTeamRequestDto
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
    }
}
