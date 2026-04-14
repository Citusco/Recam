using Remp.Models.Enums;

namespace Remp.Service.DTOs;

public class ListingCaseResponseDto
{
    public int Id { get; set; }
    public ListCaseStatus ListCaseStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}