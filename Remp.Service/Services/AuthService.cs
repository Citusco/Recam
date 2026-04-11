using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Remp.Models.Entities;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Remp.Repositories.Interfaces;

namespace Remp.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IAgentRepository _agentRepository;

    public AuthService(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IConfiguration configuration,
        IAgentRepository agentRepository
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _configuration = configuration;
        _agentRepository = agentRepository;
    }

    // Register service.
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        User user = _mapper.Map<User>(request);
        // username is as the same as email
        user.UserName = request.Email;

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Assign role or create new role
        if (!await _roleManager.RoleExistsAsync("Agent"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Agent"));
        }
        await _userManager.AddToRoleAsync(user, "Agent");

        // Create agent
        Agent agent = _mapper.Map<Agent>(request);
        agent.User = user;
        agent.Id = user.Id;
        await _agentRepository.AddAgentToDbAsync(agent);

        return GenerateToken(user, "Agent");
    }

    // Login service
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }
        
        // Verify password
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new Exception("Invalid email or password.");
        }

        // Get user's role
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Agent";

        return GenerateToken(user, role);
    }

    // Helper method to generate Jwt token.
    private AuthResponseDto GenerateToken(User user, string role)
    {
        // Jwt payload
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        // Jwt secure key.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"]));

        // Jwt token.
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Role = role
        };
    }
}
