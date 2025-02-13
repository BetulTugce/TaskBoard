namespace TaskBoard.Application.DTOs.Task
{
    // Bir gorevi ya da gorevleri listelemek icin kullanilacak DTO 
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? ReminderTime { get; set; }
        public Guid? AssignedToId { get; set; }
        //public string AssignedToName { get; set; } = string.Empty; // Kullanici adi bilgisi
        public Guid TeamId { get; set; }
        //public string TeamName { get; set; }  // Takim adi bilgisi
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
