using System;
using System.ComponentModel;
using Remp.Models.Enums;

namespace Remp.Models.Entities;

public class StatusHistory
{
    public int Id { get; set; }
    public int ListingCaseId { get; set; }
    public ListingCase ListingCase { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ListCaseStatus OldStatus { get; set; }
    public ListCaseStatus NewStatus { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.Now;
}