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

        [HttpGet]
        public async Task<IActionResult> GetAllTeams([FromQuery] GetAllTeamsRequestDto request)
        {
            GetAllTeamsResponseDto response = new GetAllTeamsResponseDto
            {
                Teams = await _teamService.GetTeamsByUserIdAsync(request),
                TotalTeamsCount = await _teamService.GetTotalCountByUserIdAsync(request.UserId)
            };
            return Ok(response);
        }
    }
}
