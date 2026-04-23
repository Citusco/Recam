using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Agent")]
        public async Task<ActionResult<AgentDetailsDto>> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            AgentDetailsDto agentDetailsDto = await _userService.GetUserAsync(userId);
            return Ok(agentDetailsDto);
        }
    }
}
