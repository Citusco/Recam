using System;

namespace Recam.Models.Entities;

public class Agent
{
    
    public string Id { get; set; }
    public string AgentFirstName { get; set; }
    public string AgentLastName { get; set; }
    public string AvatarUrl { get; set; }
    public string CompanyName { get; set; }

    // An agent is an user.
    public User User { get; set; }
    public ICollection<AgentListingCase> AgentListingCases { get; set; }
    public ICollection<AgentPhotographyCompany> AgentPhotographyCompanies { get; set; }
}
