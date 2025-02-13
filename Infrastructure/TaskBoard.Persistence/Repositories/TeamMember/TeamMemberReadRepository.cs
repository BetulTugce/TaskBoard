using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Entities.Identity;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamMemberReadRepository : ITeamMemberReadRepository
    {
        readonly private TaskBoardDbContext _context;

        public TeamMemberReadRepository(TaskBoardDbContext context)
        {
            _context = context;
        }

        // Belirli bir teame ait tum userlari getirir..
        public async Task<IEnumerable<ApplicationUser>> GetTeamMembersByTeamIdAsync(Guid teamId)
        {
            // Team IDsine gore takim uyelerini aliniyor..
            var team = await _context.Teams.Include(t => t.Members)
                                             .FirstOrDefaultAsync(t => t.Id == teamId);

            return team?.Members ?? new List<ApplicationUser>(); // Listeyi gonderir..
        }

        // Belirli bir usera ait tum teamleri getirir..
        public async Task<IEnumerable<Team>> GetTeamsByUserIdAsync(Guid userId)
        {
            // User IDsine gore teamleri aliyor..
            var user = await _context.Users.Include(u => u.Teams)
                                             .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Teams ?? new List<Team>(); // Listeyi gonderir..
        }
    }
}
