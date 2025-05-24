using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.Common;
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
        public async Task<Result<bool>> CreateAsync(CreateTeamRequestDto teamDto)
        {
            var newTeam = _mapper.Map<Team>(teamDto);
            await _teamWriteRepository.AddAsync(newTeam);
            await _teamWriteRepository.SaveAsync();
            return Result<bool>.Success(true, "Team created successfully.");
        }

        // Kullanici idsi ile eşleşen takimlari page-sizea gore getirir..
        public async Task<Result<List<TeamResponseDto>>> GetTeamsByUserIdAsync(GetAllTeamsRequestDto requestDto, Guid userId)
        {
            var teams = await _teamReadRepository.GetTeamsByUserIdAsync(userId, requestDto.Page, requestDto.Size);

            //return teams.Select(team => new TeamResponseDto
            //{
            //    Id = team.Id,
            //    Name = team.Name,
            //    CreatedAt = team.CreatedAt
            //}).ToList();
            return Result<List<TeamResponseDto>>.Success(_mapper.Map<List<TeamResponseDto>>(teams));
        }

        // Kullanici idsi ile eşleşen takimlarin sayisini getirir..
        public async Task<Result<int>> GetTotalCountByUserIdAsync(Guid userId)
        {
            var count = await _teamReadRepository.GetTotalCountByUserIdAsync(userId);
            return Result<int>.Success(count);
        }

        // Parametredeki teamIdye sahip takimi siler..
        public async Task<Result<bool>> RemoveAsync(Guid teamId)
        {
            var isRemoved = await _teamWriteRepository.RemoveAsync(teamId);
            if (!isRemoved)
            {
                return Result<bool>.Failure("Team not found or could not be deleted.", ErrorCode.NotFound);
            }

            await _teamWriteRepository.SaveAsync();
            return Result<bool>.Success(true, "Team deleted successfully.");
        }

        // TeamIdye gore takimi siler.. (Takim yoneticisi ise..)
        public async Task<Result<bool>> RemoveIfManagerAsync(RemoveTeamRequestDto request)
        {
            // Silinmek istenen takimi veritabaninda bulur..
            var team = await _teamReadRepository.GetByIdAsync(request.TeamId);

            if (team == null)
            {
                return Result<bool>.Failure("Team not found.", ErrorCode.NotFound);
            }

            // Kullanicinin bu takimin yoneticisi olup olmadigi kontrol ediliyor..
            if (team.ManagerId != request.UserId)
            {
                return Result<bool>.Failure("You're not authorized to delete this team.", ErrorCode.Forbidden); // Kullanici yetkili degil
            }

            // Yetkiliyse takim siliniyor..
            bool isDeleted = await _teamWriteRepository.RemoveAsync(request.TeamId);

            if (!isDeleted)
            {
                return Result<bool>.Failure("Failed to delete team.", ErrorCode.InternalError);
            }

            await _teamWriteRepository.SaveAsync();
            return Result<bool>.Success(true, "Team deleted successfully.");
        }
    }
}
