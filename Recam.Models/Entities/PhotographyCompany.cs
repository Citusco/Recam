using System;

namespace Recam.Models.Entities;

public class PhotographyCompany
{
    public string Id { get; set; }
    public string PhotographyCompanyName { get; set; }
    public ICollection<AgentPhotographyCompany> AgentPhotographyCompanies { get; set; }
}
