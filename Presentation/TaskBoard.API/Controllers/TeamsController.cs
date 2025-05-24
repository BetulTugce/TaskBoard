using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TaskBoard.API.Extensions;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.DTOs.Team;

namespace TaskBoard.API.Controllers
{
    [Authorize(AuthenticationSchemes = "UserAuthScheme")]
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
            var result = await _teamService.CreateAsync(request);
            if (!result.Succeeded)
                return this.MapErrorResult(result);

            //return StatusCode((int)HttpStatusCode.Created, new { message = result.Message });
            return Created(string.Empty, new { message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeams([FromQuery] GetAllTeamsRequestDto request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized(new { error = "You are not authorized to perform this action." });
            var teamsResult = await _teamService.GetTeamsByUserIdAsync(request, userId);
            if (!teamsResult.Succeeded)
                return this.MapErrorResult(teamsResult);

            var countResult = await _teamService.GetTotalCountByUserIdAsync(userId);
            if (!countResult.Succeeded)
                return this.MapErrorResult(countResult);

            var response = new GetAllTeamsResponseDto
            {
                Teams = teamsResult.Data,
                TotalTeamsCount = countResult.Data
            };

            return Ok(response);
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> Delete(Guid teamId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new { error = "You are not authorized to perform this action." });
            }

            // Silme islemini gerceklestirir..
            var result = await _teamService.RemoveIfManagerAsync(new RemoveTeamRequestDto() { TeamId = teamId, UserId = userId });

            if (!result.Succeeded)
            {
                return this.MapErrorResult(result);
            }

            return Ok();
        }
    }
}
