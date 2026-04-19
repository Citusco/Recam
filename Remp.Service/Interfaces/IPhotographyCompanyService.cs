using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IPhotographyCompanyService
{
    Task<AgentPhotographyCompanyResponseDto> AssignAgentToCompany(string companyId, string userId);
}
