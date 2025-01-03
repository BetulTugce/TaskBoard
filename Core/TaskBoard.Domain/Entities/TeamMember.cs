using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskBoard.Domain.Entities.Common;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
