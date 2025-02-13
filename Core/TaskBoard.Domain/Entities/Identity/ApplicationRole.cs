using Microsoft.AspNetCore.Identity;

namespace TaskBoard.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        virtual public DateTime? UpdatedAt { get; set; }
    }
}
