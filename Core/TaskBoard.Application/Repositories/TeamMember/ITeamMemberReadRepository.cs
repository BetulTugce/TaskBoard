using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Application.Repositories
{
    public interface ITeamMemberReadRepository
    {
        Task<IEnumerable<ApplicationUser>> GetTeamMembersByTeamId(Guid teamId);
        Task<IEnumerable<Team>> GetTeamsByUserId(Guid userId);
    }
}
