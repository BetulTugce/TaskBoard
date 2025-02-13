using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Task;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateTaskRequestDto request)
        {
            await _taskService.CreateAsync(request);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
