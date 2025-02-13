using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Repositories
{
    public interface ITeamReadRepository : IReadRepository<Team>
    {
        Task<List<Team>> GetTeamsByUserIdAsync(Guid userId, int page, int size);
        Task<int> GetTotalCountByUserIdAsync(Guid userId);
    }
}
