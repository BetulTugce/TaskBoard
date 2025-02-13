namespace TaskBoard.Application.DTOs.Task
{
    // Yeni gorev olusturmak icin kullanilacak DTO
    public class CreateTaskRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.Pending;
        public DateTime Deadline { get; set; } = DateTime.UtcNow.AddDays(1);
        public DateTime? ReminderTime { get; set; }

        // Gorevin atanmis oldugu kullanici
        public Guid? AssignedToId { get; set; }

        // Gorevin ait oldugu takim
        public required Guid TeamId { get; set; }
    }
}
