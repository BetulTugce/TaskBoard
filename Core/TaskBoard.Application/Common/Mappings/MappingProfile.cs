using AutoMapper;
using TaskBoard.Application.DTOs.Task;
using TaskBoard.Application.DTOs.Team;
using TaskBoard.Application.DTOs.User;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Domain.Entities.Task, CreateTaskRequestDto>().ReverseMap();
            CreateMap<Team, CreateTeamRequestDto>().ReverseMap();
            CreateMap<Team, TeamResponseDto>().ReverseMap();
            CreateMap<ApplicationUser, LoginUserDto>().ReverseMap();
        }
    }
}
