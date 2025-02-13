using AutoMapper;
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

        public async Task CreateAsync(CreateTeamRequestDto teamDto)
        {
            var newTeam = _mapper.Map<Team>(teamDto);
            await _teamWriteRepository.AddAsync(newTeam);
            await _teamWriteRepository.SaveAsync();
        }
    }
}
