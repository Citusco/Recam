using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            AuthResponseDto authResponseDto = await _service.RegisterAsync(requestDto);
            AppendAuthCookie(authResponseDto);
            return CreatedAtAction(nameof(Register), ToClientAuthResponse(authResponseDto));
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(
            [FromBody] RegisterAdminRequestDto requestDto
        )
        {
            AuthResponseDto authResponseDto = await _service.RegisterAdminAsync(requestDto);
            AppendAuthCookie(authResponseDto);
            return CreatedAtAction(nameof(RegisterAdmin), ToClientAuthResponse(authResponseDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            AuthResponseDto authResponseDto = await _service.LoginAsync(requestDto);
            AppendAuthCookie(authResponseDto);
            return Ok(ToClientAuthResponse(authResponseDto));
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            string? email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
            string? role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(
                new
                {
                    Id = userId,
                    Email = email,
                    Role = role,
                }
            );
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token", GetAuthCookieOptions());
            return NoContent();
        }

        private void AppendAuthCookie(AuthResponseDto authResponseDto)
        {
            Response.Cookies.Append(
                "access_token",
                authResponseDto.Token,
                GetAuthCookieOptions(authResponseDto.Expiration)
            );
        }

        private static CookieOptions GetAuthCookieOptions(DateTime? expires = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/",
            };
        }

        private static object ToClientAuthResponse(AuthResponseDto authResponseDto)
        {
            return new { authResponseDto.Expiration, authResponseDto.Role };
        }
    }
}
