using Remp.Models.Entities;

namespace Remp.Repositories.Interfaces;

public interface IAgentRepository
{
    Task<Agent> AddAgentToDbAsync (Agent agent);
    Task<Agent> GetAgentAsync (string id);
}
