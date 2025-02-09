using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamMemberWriteRepository : WriteRepository<TeamMember>, ITeamMemberWriteRepository
    {
        readonly private TaskBoardDbContext _context;

        public TeamMemberWriteRepository(TaskBoardDbContext context) : base(context)
        {
            _context = context;
        }

        // Kullaniciyi bir takima ekler..
        public async System.Threading.Tasks.Task AddUserToTeamAsync(Guid userId, Guid teamId)
        {
            // Oncelikle kullanici ve takimin var olup olmadigini kontrol ediliyor..
            var team = await _context.Teams.FindAsync(teamId);
            var user = await _context.Users.FindAsync(userId);

            // Eger takim veya kullanici bulunmazsa, islem yapilmaz..
            if (team == null || user == null)
            {
                throw new ArgumentException("Team or User not found.");
            }

            // Yeni bir TeamMember olusturuluyor..
            var teamMember = new TeamMember
            {
                UserId = userId,
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            };

            // TeamMember ekleniyor..
            await _context.TeamMembers.AddAsync(teamMember);
            //await _context.SaveChangesAsync();
        }

        // Kullaniciyi bir takimdan siler..
        public async System.Threading.Tasks.Task RemoveUserFromTeamAsync(Guid userId, Guid teamId)
        {
            // Takim ve kullaniciya ait TeamMember kaydini arıyor..
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.UserId == userId && tm.TeamId == teamId);

            // Eger boyle bir iliski yoksa, islem yapilmaz..
            if (teamMember == null)
            {
                throw new ArgumentException("The user is not a member of this team.");
            }

            // TeamMember siliniyor..
            _context.TeamMembers.Remove(teamMember);
            //await _context.SaveChangesAsync();
        }
    }
}
