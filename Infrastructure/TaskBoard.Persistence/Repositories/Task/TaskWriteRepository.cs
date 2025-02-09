using TaskBoard.Application.Repositories;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence.Repositories.Task
{
    public class TaskWriteRepository : WriteRepository<Domain.Entities.Task>, ITaskWriteRepository
    {
        public TaskWriteRepository(TaskBoardDbContext context) : base(context)
        {
        }
    }
}
