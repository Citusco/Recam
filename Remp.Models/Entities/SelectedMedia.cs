namespace Remp.Models.Entities;

public class SelectedMedia
{
    public int Id { get; set; }
    public int MediaAssetId { get; set; }
    public MediaAsset MediaAsset { get; set; }
    public int ListingCaseId { get; set; }
    public ListingCase ListingCase { get; set; }
    public string AgentId { get; set; }
    public Agent Agent { get; set; }
    public DateTime SelectedAt { get; set; }
}
