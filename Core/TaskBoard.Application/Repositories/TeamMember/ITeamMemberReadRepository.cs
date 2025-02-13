using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Application.Repositories
{
    public interface ITeamMemberReadRepository
    {
        Task<IEnumerable<ApplicationUser>> GetTeamMembersByTeamIdAsync(Guid teamId);
        Task<IEnumerable<Team>> GetTeamsByUserIdAsync(Guid userId);
    }
}
