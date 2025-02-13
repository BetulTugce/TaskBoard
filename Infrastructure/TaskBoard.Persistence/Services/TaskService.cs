using AutoMapper;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Task;
using TaskBoard.Application.Repositories;

namespace TaskBoard.Persistence.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskWriteRepository _taskWriteRepository;
        private readonly ITaskReadRepository _taskReadRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskWriteRepository taskWriteRepository, ITaskReadRepository taskReadRepository, IMapper mapper)
        {
            _taskWriteRepository = taskWriteRepository;
            _taskReadRepository = taskReadRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateTaskRequestDto taskDto)
        {
            var newTask = _mapper.Map<Domain.Entities.Task>(taskDto);
            await _taskWriteRepository.AddAsync(newTask);
            await _taskWriteRepository.SaveAsync();
        }
    }
}
