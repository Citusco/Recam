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
}
