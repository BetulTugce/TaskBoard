using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Repositories;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamMemberWriteRepository : ITeamMemberWriteRepository
    {
        readonly private TaskBoardDbContext _context;

        public TeamMemberWriteRepository(TaskBoardDbContext context)
        {
            _context = context;
        }

        // Kullaniciyi bir takima ekler..
        public async System.Threading.Tasks.Task AddUserToTeamAsync(Guid userId, Guid teamId)
        {
            // Oncelikle kullanici ve takimin var olup olmadigini kontrol ediliyor..
            var team = await _context.Teams
               .Include(t => t.Members) // Takım uyeleri yukleniyor..
               .FirstOrDefaultAsync(t => t.Id == teamId);
            var user = await _context.Users.FindAsync(userId);

            // Eger takim veya kullanici bulunmazsa, islem yapilmaz..
            if (team == null || user == null)
            {
                throw new ArgumentException("Team or User not found.");
            }

            // Kullanici zaten ekli degilse ekler
            if (!team.Members.Contains(user))
            {
                // Yeni bir TeamMember olusturuluyor.. EF Core otomatik olarak ara tabloyu yonetecek.
                team.Members.Add(user);

                await _context.SaveChangesAsync(); // Veritabanina kaydediliyor..
            }
        }

        // Kullaniciyi bir takimdan siler..
        public async System.Threading.Tasks.Task RemoveUserFromTeamAsync(Guid userId, Guid teamId)
        {
            // Takim kaydi araniyor..
            var team = await _context.Teams.Include(t => t.Members).FirstOrDefaultAsync(t => t.Id == teamId);
            var user = await _context.Users.FindAsync(userId);

            // Eger boyle bir iliski yoksa, islem yapilmaz..
            if (team == null || user == null)
            {
                throw new ArgumentException("Team or User not found.");
            }

            // Kullanici takimda varsa cikarir
            if (team.Members.Contains(user))
            {
                // Kullanici takimdan siliniyor..
                team.Members.Remove(user);
                await _context.SaveChangesAsync(); // Degisiklikler veritabanina kaydediliyor..
            }
        }
    }
}
