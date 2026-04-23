namespace Remp.Service.DTOs;

public class RegisterAdminRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhotographyCompanyName { get; set; }
}
