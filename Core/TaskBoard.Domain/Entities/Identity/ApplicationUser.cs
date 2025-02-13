using Microsoft.AspNetCore.Identity;

namespace TaskBoard.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        virtual public DateTime? UpdatedAt { get; set; }

        // Kullaniciya atanmis gorevler
        public virtual ICollection<Task> Tasks { get; set; }

        // Kullanicinin yonetici oldugu takimlar
        public virtual ICollection<Team> ManagedTeams { get; set; }

        // Kullanicinin dahil oldugu takimlar (many-to-many ilişki)
        public virtual ICollection<Team> Teams { get; set; }

    }
}
