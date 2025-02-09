using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Entities.Common;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Persistence.Contexts
{
    public class TaskBoardDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public TaskBoardDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        //public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Task iliskileri 

            // ApplicationUser (Parent) -> Task (Child)
            builder.Entity<Domain.Entities.Task>()
                .HasOne(t => t.AssignedTo) // Bir Task, bir Usera atanabilir.
                .WithMany(u => u.Tasks) // Bir User, birden fazla Taske sahip olabilir.
                .HasForeignKey(t => t.AssignedToId) // Task tablosunda AssignedToId foreign key olarak tanimli.
                .OnDelete(DeleteBehavior.SetNull); // User silinirse Taskin atandigi kisi idsi (AssignedToId) null olur.

            // Team (Parent) -> Task (Child)
            builder.Entity<Domain.Entities.Task>()
                .HasOne(t => t.Team) // Her task, bir Teame ait olabilir.
                .WithMany(team => team.Tasks) // Bir Team, birden fazla Task'e sahip olabilir.
                .HasForeignKey(t => t.TeamId) // Task tablosunda TeamId foreign key olarak tanimli.
                .OnDelete(DeleteBehavior.Cascade); // Team silindiginde, bagli Tasks otomatik silinir.
            #endregion

            #region Team iliskileri

            // Team -> Manager
            builder.Entity<Team>()
                .HasOne(t => t.Manager) // Her Teamin bir Manageri olabilir.
                .WithMany(u => u.ManagedTeams) // Bir User birden fazla Teamin Manageri olabilir.
                .HasForeignKey(t => t.ManagerId) // Team tablosunda ManagerId foreign key olarak tanimli.
                .OnDelete(DeleteBehavior.Restrict); // Managerin silinmesi engellenir..
            #endregion

            #region Team ve ApplicationUser (TeamMember) iliskisi
            builder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithMany(u => u.Teams);
            #endregion
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Veritabani kayitlarıini kaydetmeden once CreatedAt ve UpdatedAt alanlarini otomatik olarak ayarlar..

            var datas = ChangeTracker
                 .Entries<BaseEntity>(); // BaseEntityden tureyen tum varliklari alir..

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedAt = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedAt = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            // Degisiklikleri veritabanina kaydeder..
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
