namespace TaskBoard.Application.DTOs.Team
{
    public class GetAllTeamsResponseDto
    {
        public List<TeamResponseDto> Teams { get; set; }
        public int TotalTeamsCount { get; set; }
    }
}
