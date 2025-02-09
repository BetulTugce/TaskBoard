using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Repositories
{
    public interface ITeamMemberReadRepository : IReadRepository<TeamMember>
    {
        Task<IEnumerable<TeamMember>> GetTeamMembersByTeamId(Guid teamId);
        Task<IEnumerable<TeamMember>> GetTeamsByUserId(Guid userId);
    }
}
