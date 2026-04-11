using System;
using Remp.Models.Entities;
using Remp.Repositories.Interfaces;
using Remp.DataAccess.Data;

namespace Remp.Repositories.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly RempDbContext _dbContext;

    public AgentRepository (RempDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Agent> AddAgentToDbAsync(Agent agent)
    {
        await _dbContext.Agents.AddAsync(agent);

        int changes = await _dbContext.SaveChangesAsync();

        if (changes > 0)
        {
            return agent;
        }
        else
        {
            throw new Exception("Add agent failed");
        }
    }
}
