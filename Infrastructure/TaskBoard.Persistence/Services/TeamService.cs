using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Team;
using TaskBoard.Application.Repositories;
using TaskBoard.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskBoard.Persistence.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamWriteRepository _teamWriteRepository;
        private readonly ITeamReadRepository _teamReadRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamWriteRepository teamWriteRepository, ITeamReadRepository teamReadRepository, IMapper mapper)
        {
            _teamWriteRepository = teamWriteRepository;
            _teamReadRepository = teamReadRepository;
            _mapper = mapper;
        }

        // Yeni bir takim olusturur..
        public async Task CreateAsync(CreateTeamRequestDto teamDto)
        {
            var newTeam = _mapper.Map<Team>(teamDto);
            await _teamWriteRepository.AddAsync(newTeam);
            await _teamWriteRepository.SaveAsync();
        }

        // Kullanici idsi ile eşleşen takimlari page-sizea gore getirir..
        public async Task<List<TeamResponseDto>> GetTeamsByUserIdAsync(GetAllTeamsRequestDto requestDto)
        {
            var teams = await _teamReadRepository.GetTeamsByUserIdAsync(requestDto.UserId, requestDto.Page, requestDto.Size);

            //return teams.Select(team => new TeamResponseDto
            //{
            //    Id = team.Id,
            //    Name = team.Name,
            //    CreatedAt = team.CreatedAt
            //}).ToList();
            return _mapper.Map<List<TeamResponseDto>>(teams);
        }

        // Kullanici idsi ile eşleşen takimlarin sayisini getirir..
        public async Task<int> GetTotalCountByUserIdAsync(Guid userId)
        {
            return await _teamReadRepository.GetTotalCountByUserIdAsync(userId);
        }
    }
}
