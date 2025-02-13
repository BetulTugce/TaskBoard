using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamReadRepository : ReadRepository<Team>, ITeamReadRepository
    {
        public TeamReadRepository(TaskBoardDbContext context) : base(context)
        {
        }

        public async Task<List<Team>> GetTeamsByUserIdAsync(Guid userId, int page, int size)
        {
            return await _context.Teams
           .Where(team => team.Members.Any(user => user.Id == userId))
           .Skip((page - 1) * size)
           .Take(size)
           .ToListAsync();
        }

        public async Task<int> GetTotalCountByUserIdAsync(Guid userId)
        {
            return await _context.Teams
            .Where(team => team.Members.Any(user => user.Id == userId))
            .CountAsync();
        }
    }
}
