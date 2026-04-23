using System;

namespace Remp.Service.DTOs;

public class AgentResponseDto : UserResponseDto
{
    public string Id { get; set; }
    public List<string> CompanyNames { get; set; }
}
