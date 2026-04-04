namespace Recam.Models.Entities;

public class AgentListingCase
{
    public string AgentId { get; set; }
    public Agent Agent { get; set; }
    public int ListingCaseId { get; set; }
    public ListingCase ListingCase { get; set; }
}