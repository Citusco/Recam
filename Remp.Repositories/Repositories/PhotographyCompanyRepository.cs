using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;

namespace Remp.Repositories.Repositories;

public class PhotographyCompanyRepository : IPhotographyCompanyRepository
{
    private readonly RempDbContext _dbcontext;

    public PhotographyCompanyRepository(RempDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public async Task AssignAgentToCompany(AgentPhotographyCompany agentPhotographyCompany)
    {
        _dbcontext.AgentPhotographyCompanies.Add(agentPhotographyCompany);
        int changes = await _dbcontext.SaveChangesAsync();
        if (changes == 0)
        {
            throw new InvalidOperationException("Failed to assign agent to company.");
        }
    }

    public async Task<bool> ExistsAsync(string companyId, string agentId)
    {
        return await _dbcontext.AgentPhotographyCompanies.AnyAsync(p => p.PhotographyCompanyId == companyId && p.AgentId == agentId);
    }

    public async Task<(IEnumerable<Agent>, int count)> GetAllAgentsAsync(int page, int pageSize)
    {
        int totalCount = await _dbcontext.Agents.CountAsync();
        IEnumerable<Agent> agents = await _dbcontext.Agents
            .Include(a => a.User)
            .Include(a => a.AgentPhotographyCompanies)
                .ThenInclude(apc => apc.PhotographyCompany)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (agents, totalCount);
    }

    public async Task<IEnumerable<Agent>> GetCompanyAgentsAsync(string companyId)
    {
        return await _dbcontext.AgentPhotographyCompanies
            .Where(p => p.PhotographyCompanyId == companyId)
            .Include(p => p.Agent)
                .ThenInclude(a => a.User)
            .Include(p => p.Agent)
                .ThenInclude(a => a.AgentPhotographyCompanies)
                    .ThenInclude(apc => apc.PhotographyCompany)
            .Select(p => p.Agent)
            .ToListAsync();
    }

}
