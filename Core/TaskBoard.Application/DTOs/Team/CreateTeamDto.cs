namespace TaskBoard.Application.DTOs.Team
{
    // Yeni takim olusturmak icin kullanilacak DTO
    public class CreateTeamDto
    {
        public required string Name { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
