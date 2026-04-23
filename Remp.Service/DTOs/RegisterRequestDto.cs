namespace Remp.Service.DTOs;

public class RegisterRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string AgentFirstName { get; set; }
    public required string AgentLastName { get; set; }
    public string? AvatarUrl { get; set; }
}