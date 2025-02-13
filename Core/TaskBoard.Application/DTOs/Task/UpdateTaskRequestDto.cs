namespace TaskBoard.Application.DTOs.Task
{
    // Mevcut gorevi guncellemek icin kullanilacak DTO
    public class UpdateTaskRequestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? ReminderTime { get; set; }
        public Guid? AssignedToId { get; set; }
        public Guid TeamId { get; set; }
    }
}
