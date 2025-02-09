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
    }
}
