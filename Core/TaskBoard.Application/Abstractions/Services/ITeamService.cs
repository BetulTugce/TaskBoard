using TaskBoard.Application.DTOs.Team;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface ITeamService
    {
        Task CreateAsync(CreateTeamRequestDto teamDto);
        Task<List<TeamResponseDto>> GetTeamsByUserIdAsync(GetAllTeamsRequestDto requestDto);
        Task<int> GetTotalCountByUserIdAsync(Guid userId);
    }
}
