using Remp.Models.Entities;
using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IPhotographyCompanyService
{
    Task<AgentPhotographyCompanyResponseDto> AssignAgentToCompany(string companyId, string userId);
    Task<PagedResponseDto<AgentResponseDto>> GetAllAgentsAsync(int page, int pageSize);
    Task<IEnumerable<AgentResponseDto>> GetCompanyAgentsAsync(string companyId);
}
