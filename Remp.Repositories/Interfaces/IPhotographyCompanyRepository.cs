using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IPhotographyCompanyRepository
{
    Task AssignAgentToCompany(AgentPhotographyCompany agentPhotographyCompany);
    Task<bool> ExistsAsync(string companyId, string agentId);
}