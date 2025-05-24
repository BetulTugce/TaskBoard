using TaskBoard.Application.Common;
using TaskBoard.Application.DTOs.Team;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface ITeamService
    {
        Task<Result<bool>> CreateAsync(CreateTeamRequestDto teamDto);
        Task<Result<List<TeamResponseDto>>> GetTeamsByUserIdAsync(GetAllTeamsRequestDto requestDto, Guid userId);
        Task<Result<int>> GetTotalCountByUserIdAsync(Guid userId);

        // Parametredeki teamIdye sahip takimi siler..
        Task<Result<bool>> RemoveAsync(Guid teamId);

        // TeamIdye gore siler (yonetici kontrolü yaparak)
        Task<Result<bool>> RemoveIfManagerAsync(RemoveTeamRequestDto request);
    }
}
