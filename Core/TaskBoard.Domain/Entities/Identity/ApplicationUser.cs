using Microsoft.AspNetCore.Identity;

namespace TaskBoard.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Kullaniciya atanmis gorevler
        public virtual ICollection<Task> Tasks { get; set; }

        // Kullanicinin ait oldugu takimlar
        public virtual ICollection<TeamMember> TeamMembers { get; set; }

        // Kullanicinin yonetici oldugu takimlar
        public virtual ICollection<Team> ManagedTeams { get; set; }

    }
}
