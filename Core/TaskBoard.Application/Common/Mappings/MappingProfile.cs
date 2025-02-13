using AutoMapper;
using TaskBoard.Application.DTOs.Task;

namespace TaskBoard.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Domain.Entities.Task, CreateTaskRequestDto>().ReverseMap();
        }
    }
}
