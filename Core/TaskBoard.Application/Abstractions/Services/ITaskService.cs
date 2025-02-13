using TaskBoard.Application.DTOs.Task;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface ITaskService
    {
        Task CreateAsync(CreateTaskRequestDto taskDto);
    }
}
