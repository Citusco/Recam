namespace Remp.Service.DTOs;

public class AgentDetailsDto
{
    public string UserId { get; set; }
    public string Role { get; set; }
    public IEnumerable<int> AssignedListingIds { get; set; }
}
