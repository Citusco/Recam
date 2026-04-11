using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;  
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto requestDto)
        {
            AuthResponseDto authResponseDto = await _service.RegisterAsync(requestDto);
            return CreatedAtAction(nameof(Register), authResponseDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto requestDto)
        {
            AuthResponseDto authResponseDto = await _service.LoginAsync(requestDto);
            return Ok(authResponseDto);
        }
    }
}
