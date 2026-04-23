namespace Remp.Service.DTOs;

public class UserResponseDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string AgentFirstName { get; set; }
    public string AgentLastName { get; set; }
    public string? AvatarUrl { get; set; }
}
