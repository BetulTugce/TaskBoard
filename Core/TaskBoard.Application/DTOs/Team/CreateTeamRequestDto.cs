namespace TaskBoard.Application.DTOs.Team
{
    // Yeni takim olusturmak icin kullanilacak DTO
    public class CreateTeamRequestDto
    {
        public required string Name { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
