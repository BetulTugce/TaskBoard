using TaskBoard.Application.DTOs.Team;
using System.Threading.Tasks;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface ITeamService
    {
        Task CreateAsync(CreateTeamRequestDto teamDto);
    }
}
