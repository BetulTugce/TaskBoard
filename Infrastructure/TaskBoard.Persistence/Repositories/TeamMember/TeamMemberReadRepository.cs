using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamMemberReadRepository : ReadRepository<TeamMember>, ITeamMemberReadRepository
    {
        readonly private TaskBoardDbContext _context;

        public TeamMemberReadRepository(TaskBoardDbContext context) : base(context)
        {
            _context = context;
        }

        // Belirli bir teame ait tum userlari getirir..
        public async Task<IEnumerable<TeamMember>> GetTeamMembersByTeamId(Guid teamId)
        {
            // Team IDsine gore takim uyelerini aliniyor..
            var teamMembers = await _context.TeamMembers
                                             .Where(tm => tm.TeamId == teamId)
                                             .ToListAsync();

            return teamMembers; // Listeyi gonderir..
        }

        // Belirli bir usera ait tum teamleri getirir..
        public async Task<IEnumerable<TeamMember>> GetTeamsByUserId(Guid userId)
        {
            // User IDsine gore teamleri aliyor..
            var teamMembers = await _context.TeamMembers
                                             .Where(tm => tm.UserId == userId)
                                             .ToListAsync();

            return teamMembers; // Listeyi gonderir..
        }
    }
}
