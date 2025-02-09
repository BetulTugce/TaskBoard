using TaskBoard.Application.Repositories;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories.Task
{
    public class TaskReadRepository : ReadRepository<Domain.Entities.Task>, ITaskReadRepository
    {
        public TaskReadRepository(TaskBoardDbContext context) : base(context)
        {
        }
    }
}
