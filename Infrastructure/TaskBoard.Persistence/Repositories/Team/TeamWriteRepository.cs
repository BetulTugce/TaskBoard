using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories
{
    public class TeamWriteRepository : WriteRepository<Team>, ITeamWriteRepository
    {
        public TeamWriteRepository(TaskBoardDbContext context) : base(context)
        {
        }
    }
}
