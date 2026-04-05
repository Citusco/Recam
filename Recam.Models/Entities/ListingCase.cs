using Recam.Models.Enums;

namespace Recam.Models.Entities;

public class ListingCase
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Postcode  { get; set; }
    public decimal Longitude  { get; set; }
    public decimal Latitude  { get; set; }
    public double Price  { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int Garages { get; set; }
    public double FloorArea { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
    public PropertyType PropertyType { get; set; }
    public SaleCategory SaleCategory { get; set; }
    public ListCaseStatus ListCaseStatus { get; set;}
    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<AgentListingCase> AgentListingCases { get; set; }
    public ICollection<CaseContact> CaseContacts { get; set; }
    public ICollection<MediaAsset> MediaAssets { get; set; }
}
