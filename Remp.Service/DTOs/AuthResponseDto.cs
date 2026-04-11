namespace Remp.Service.DTOs;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
    public required string Role { get; set; }
}
