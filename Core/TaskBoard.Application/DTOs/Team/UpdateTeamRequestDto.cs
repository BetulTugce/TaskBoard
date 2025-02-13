using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Application.DTOs.Team
{
    // Mevcut takimi guncellemek icin kullanilacak DTO
    public class UpdateTeamRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
