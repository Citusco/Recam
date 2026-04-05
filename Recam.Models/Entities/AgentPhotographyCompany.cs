using System;

namespace Recam.Models.Entities;

public class AgentPhotographyCompany
{
    public string AgentId { get; set; }
    public Agent Agent { get; set; }
    public string PhotographyCompanyId { get; set; }
    public PhotographyCompany PhotographyCompany { get; set; }
}
