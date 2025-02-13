using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Team;

namespace TaskBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateTeamRequestDto request)
        {
            await _teamService.CreateAsync(request);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
