using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid? Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
