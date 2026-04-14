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

        [HttpGet("getUser")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            UserResponseDto userResponseDto = await _userService.GetUserAsync(userId);
            return Ok(userResponseDto);
        }
    }
}
