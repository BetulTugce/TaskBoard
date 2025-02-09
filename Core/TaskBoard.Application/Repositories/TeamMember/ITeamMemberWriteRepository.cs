using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Repositories
{
    public interface ITeamMemberWriteRepository
    {
        System.Threading.Tasks.Task AddUserToTeamAsync(Guid userId, Guid teamId);
        System.Threading.Tasks.Task RemoveUserFromTeamAsync(Guid userId, Guid teamId);
    }
}
