using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskBoard.Domain.Entities.Common;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Domain.Entities
{
    public class Task : BaseEntity
    {
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public Enums.TaskStatus Status { get; set; }
        public DateTime Deadline { get; set; } = DateTime.UtcNow.AddDays(1);
        public DateTime? ReminderTime { get; set; }

        // Gorevin atanmis oldugu kullanici
        [ForeignKey("AssignedTo")]
        public Guid? AssignedToId { get; set; }
        public virtual ApplicationUser AssignedTo { get; set; }

        // Gorevin ait oldugu takim
        [Required]
        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
